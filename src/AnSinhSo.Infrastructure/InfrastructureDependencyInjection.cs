using AnSinhSo.Infrastructure.Persistence.Contexts;
using AnSinhSo.Infrastructure.DataImport.Options;

using AnSinhSo.Infrastructure.Persistence.Repositories;
using AnSinhSo.Infrastructure.Security;
using AnSinhSo.Infrastructure.Notifications;
using AnSinhSo.Infrastructure.DataImport.Cache;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using AnSinhSo.Application.Abstractions.Authentication;
using AnSinhSo.Application.Abstractions.Authentication.RateLimiting;
using AnSinhSo.Infrastructure.Authentication;
using AnSinhSo.Application.Authorization.Abstractions;
using AnSinhSo.Domain.Interfaces.Authorization;
using AnSinhSo.Infrastructure.Authorization;
using AnSinhSo.Infrastructure.Caching;
using AnSinhSo.Infrastructure.Audit;
using Microsoft.AspNetCore.Authorization;

namespace AnSinhSo.Infrastructure;

public static class InfrastructureDependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AnSinhSoDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(AnSinhSoDbContext).Assembly.FullName)));

        services.AddSecurityInfrastructure();

        // Domain UoW
        services.AddScoped<AnSinhSo.Domain.Interfaces.IUnitOfWork, UnitOfWork>();
        services.AddScoped<AnSinhSo.Application.Abstractions.Security.ISessionCleanupService, SessionCleanupService>();

        // Specialized Repositories (Domain)
        services.AddScoped<AnSinhSo.Domain.Aggregates.CitizenAggregate.ICitizenRepository, CitizenRepository>();
        services.AddScoped<AnSinhSo.Domain.Aggregates.CitizenIdentityAggregate.ICitizenIdentityRepository, CitizenIdentityRepository>();
        services.AddScoped<AnSinhSo.Domain.Interfaces.IOtpVerificationRepository, OtpVerificationRepository>();
        services.AddScoped<AnSinhSo.Domain.Aggregates.HouseholdAggregate.IHouseholdRepository, HouseholdRepository>();
        services.AddScoped<AnSinhSo.Domain.Aggregates.PaymentAggregate.IPaymentRepository, PaymentRepository>();
        services.AddScoped<AnSinhSo.Domain.Aggregates.PolicyAggregate.IPolicyRepository, PolicyRepository>();
        services.AddScoped<AnSinhSo.Domain.Aggregates.WelfareGroupAggregate.IWelfareGroupRepository, WelfareGroupRepository>();
        services.AddScoped<AnSinhSo.Domain.Aggregates.WelfareProgramAggregate.IWelfareProgramRepository, WelfareProgramRepository>();
        services.AddScoped<AnSinhSo.Domain.Aggregates.WelfareCaseAggregate.IWelfareCaseRepository, WelfareCaseRepository>();
        services.AddScoped<AnSinhSo.Domain.Interfaces.IUserRepository, UserRepository>();
        services.AddScoped<AnSinhSo.Domain.Aggregates.RelationshipTypeAggregate.IRelationshipTypeRepository, RelationshipTypeRepository>();
        services.AddScoped<AnSinhSo.Domain.Aggregates.PaymentPointAggregate.IPaymentPointRepository, PaymentPointRepository>();

        // OTP Security & Notifications
        services.AddSingleton<AnSinhSo.Application.Abstractions.Security.IOtpGenerator, OtpGenerator>();
        services.AddSingleton<AnSinhSo.Application.Abstractions.Security.IHashProvider, HashProvider>();
        services.AddTransient<AnSinhSo.Application.Abstractions.Notifications.IOtpNotificationService, AnSinhSo.Infrastructure.Notifications.ConsoleOtpNotificationService>();

        // JWT Authentication (AD #86, AD #91, AD #95)
        services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.SectionName));
        services.Configure<ImportOptions>(configuration.GetSection(ImportOptions.SectionName));
        services.AddSingleton<AnSinhSo.Application.Abstractions.Authentication.IJwtProvider, JwtProvider>();
        services.AddSingleton<AnSinhSo.Application.Abstractions.Authentication.ITokenGenerator, RefreshTokenGenerator>();

        var jwtOptions = configuration.GetSection(JwtOptions.SectionName).Get<JwtOptions>() ?? new JwtOptions();
        
        if (string.IsNullOrEmpty(jwtOptions.SecretKey) || Encoding.UTF8.GetByteCount(jwtOptions.SecretKey) < 32)
        {
            throw new InvalidOperationException("JwtOptions.SecretKey must be at least 32 bytes (256 bits).");
        }
        
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtOptions.Issuer,
                    ValidAudience = jwtOptions.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecretKey)),
                    ClockSkew = TimeSpan.Zero // AD #95: Enforce exact expiration time
                };

                options.Events = new JwtBearerEvents
                {
                    OnTokenValidated = async context =>
                    {
                        var sidClaim = context.Principal?.FindFirst(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sid)?.Value;

                        if (string.IsNullOrEmpty(sidClaim) || !Guid.TryParse(sidClaim, out var sessionId))
                        {
                            context.Fail("Invalid session ID in token.");
                            return;
                        }

                        var securityRepository = context.HttpContext.RequestServices.GetRequiredService<AnSinhSo.Domain.Interfaces.ISecurityRepository>();
                        var deviceSession = await securityRepository.GetDeviceSessionByIdAsync(new AnSinhSo.Domain.Aggregates.SecurityAggregate.ValueObjects.DeviceSessionId(sessionId), context.HttpContext.RequestAborted);

                        if (deviceSession == null || !deviceSession.IsActive())
                        {
                            context.Fail("Device session is invalid or inactive.");
                            return;
                        }

                        var userRepository = context.HttpContext.RequestServices.GetRequiredService<AnSinhSo.Domain.Interfaces.IUserRepository>();
                        var user = await userRepository.GetByIdAsync(new AnSinhSo.Domain.Aggregates.UserAggregate.UserId(deviceSession.UserId), context.HttpContext.RequestAborted);
                        if (user == null || user.IsLocked)
                        {
                            context.Fail("User account is not active or locked.");
                            return;
                        }

                        if (deviceSession.SecurityStamp != user.SecurityStamp)
                        {
                            context.Fail("Security stamp changed.");
                            return;
                        }
                        if (!deviceSession.RefreshTokenId.HasValue)
                        {
                            context.Fail("Device session has no refresh token.");
                            return;
                        }

                        var refreshToken = await securityRepository.GetRefreshTokenAsync(
                            deviceSession.RefreshTokenId.Value,
                            context.HttpContext.RequestAborted);

                        if (refreshToken == null)
                        {
                            context.Fail("Refresh token associated with the session was not found.");
                            return;
                        }

                        if (refreshToken.CitizenIdentityId.HasValue)
                        {
                            var identityRepository = context.HttpContext.RequestServices
                                .GetRequiredService<AnSinhSo.Domain.Aggregates.CitizenIdentityAggregate.ICitizenIdentityRepository>();

                            var identity = await identityRepository.GetByIdAsync(
                                new AnSinhSo.Domain.Aggregates.CitizenIdentityAggregate.CitizenIdentityId(
                                    refreshToken.CitizenIdentityId.Value),
                                context.HttpContext.RequestAborted);

                            if (identity == null ||
                                identity.Status != AnSinhSo.Domain.Aggregates.CitizenIdentityAggregate.Enumerations.IdentityStatus.Verified)
                            {
                                context.Fail("Citizen identity is not verified.");
                                return;
                            }
                        }
                    }
                };
            });

        // Data Import Pipeline
        services.AddScoped<ILookupCacheService, LookupCacheService>();
        services.AddScoped<AnSinhSo.Application.DataImport.IDataImportService, AnSinhSo.Infrastructure.DataImport.DataImportService>();
        services.AddSingleton<AnSinhSo.Infrastructure.DataImport.Normalization.ICsvStringNormalizer, AnSinhSo.Infrastructure.DataImport.Normalization.CsvStringNormalizer>();
        services.AddScoped<AnSinhSo.Infrastructure.DataImport.Mappers.IWelfareGroupMapper, AnSinhSo.Infrastructure.DataImport.Mappers.WelfareGroupMapper>();
        services.AddScoped<AnSinhSo.Infrastructure.DataImport.Mappers.ICitizenMapper, AnSinhSo.Infrastructure.DataImport.Mappers.CitizenMapper>();
        services.AddScoped<AnSinhSo.Infrastructure.DataImport.Mappers.IHouseholdMapper, AnSinhSo.Infrastructure.DataImport.Mappers.HouseholdMapper>();
        services.AddScoped<AnSinhSo.Infrastructure.DataImport.Mappers.IPolicyMapper, AnSinhSo.Infrastructure.DataImport.Mappers.PolicyMapper>();
        services.AddScoped<AnSinhSo.Infrastructure.DataImport.Mappers.IPaymentMapper, AnSinhSo.Infrastructure.DataImport.Mappers.PaymentMapper>();

        // Authorization Repositories (Domain)
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IPermissionRepository, PermissionRepository>();
        services.AddScoped<IPermissionGroupRepository, PermissionGroupRepository>();
        services.AddScoped<IUserRoleRepository, UserRoleRepository>();

        // Authorization Caching & Evaluation (AD #114)
        services.AddSingleton<AnSinhSo.Application.Abstractions.Caching.IAuthorizationCacheService, AuthorizationCacheService>();
        services.AddTransient<IPermissionResolver, PermissionResolver>();
        
        services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
        services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();
        
        // Map Queries
        services.AddScoped<AnSinhSo.Application.Map.Queries.IMapQueryService, AnSinhSo.Infrastructure.Persistence.Queries.MapQueryService>();

        // Notifications
        services.AddScoped<AnSinhSo.Domain.Aggregates.NotificationAggregate.INotificationRepository, NotificationRepository>();
        services.AddScoped<AnSinhSo.Application.Notifications.Services.INotificationQueryService, AnSinhSo.Infrastructure.Persistence.Queries.NotificationQueryService>();
        services.AddScoped<AnSinhSo.Application.Notifications.Services.INotificationDispatcher, AnSinhSo.Infrastructure.Services.Notifications.NotificationDispatcher>();
        services.AddScoped<AnSinhSo.Application.Notifications.Services.IZaloNotificationService, AnSinhSo.Infrastructure.Services.Notifications.ZaloNotificationService>();

        // AI Decision Support Engine
        services.AddScoped<AnSinhSo.Domain.Interfaces.IAiRecommendationRepository, AnSinhSo.Infrastructure.Persistence.Repositories.AI.AiRecommendationRepository>();
        services.AddScoped<AnSinhSo.Application.AI.Queries.Common.IAiQueryService, AnSinhSo.Infrastructure.Persistence.Queries.AiQueryService>();
        services.AddScoped<AnSinhSo.Application.AI.Services.IAiAnalysisService, AnSinhSo.Infrastructure.Services.AI.AiAnalysisService>();
        
        // AI Rules Registration
        services.AddTransient<AnSinhSo.Application.AI.Rules.IAiRule<AnSinhSo.Application.AI.Rules.Contexts.AiHouseholdContext>, AnSinhSo.Application.AI.Rules.Households.LowIncomeRule>();
        services.AddTransient<AnSinhSo.Application.AI.Rules.IAiRule<AnSinhSo.Application.AI.Rules.Contexts.AiHouseholdContext>, AnSinhSo.Application.AI.Rules.Households.VulnerableMembersRule>();
        services.AddTransient<AnSinhSo.Application.AI.Rules.IAiRule<AnSinhSo.Application.AI.Rules.Contexts.AiCitizenContext>, AnSinhSo.Application.AI.Rules.Citizens.MissingIdentityRule>();
        services.AddTransient<AnSinhSo.Application.AI.Rules.IAiRule<AnSinhSo.Application.AI.Rules.Contexts.AiCitizenContext>, AnSinhSo.Application.AI.Rules.Citizens.ElderlyWithoutSupportRule>();

        // Register IDistributedCache and ICurrentUser
        var redisConnectionString = configuration.GetConnectionString("Redis");
        if (!string.IsNullOrEmpty(redisConnectionString))
        {
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = redisConnectionString;
                options.InstanceName = "AnSinhSo_";
            });
        }
        else
        {
            services.AddDistributedMemoryCache();
        }
        services.AddHttpContextAccessor();
        services.AddScoped<AnSinhSo.Domain.Interfaces.ICurrentUser, CurrentUser>();

        // Rate Limiting
        services.AddScoped<IOtpRateLimitService, OtpRateLimitService>();
        services.AddSingleton<IRateLimitKeyBuilder, RateLimitKeyBuilder>();

        // Zalo OA
        services.Configure<AnSinhSo.Infrastructure.Services.Zalo.ZaloOptions>(configuration.GetSection(AnSinhSo.Infrastructure.Services.Zalo.ZaloOptions.SectionName));
        services.AddScoped<AnSinhSo.Domain.Interfaces.Repositories.IZaloUserRepository, AnSinhSo.Infrastructure.Persistence.Repositories.ZaloUserRepository>();
        services.AddScoped<AnSinhSo.Application.Zalo.IZaloTokenManager, AnSinhSo.Infrastructure.Services.Zalo.ZaloTokenManager>();
        services.AddScoped<AnSinhSo.Application.Zalo.IZaloOAService, AnSinhSo.Infrastructure.Services.Zalo.ZaloOAService>();

        // Security & OTP
        services.Configure<AnSinhSo.Application.Authentication.Citizen.OtpOptions>(configuration.GetSection("OtpOptions"));
        services.AddScoped<AnSinhSo.Domain.Interfaces.ISecurityRepository, AnSinhSo.Infrastructure.Persistence.Repositories.SecurityRepository>();
        services.AddHttpClient<IOtpProvider, AnSinhSo.Infrastructure.Authentication.Providers.ZaloOtpProvider>(client =>
        {
            client.Timeout = TimeSpan.FromSeconds(30);
            client.DefaultRequestHeaders.Add("User-Agent", "AnSinhSo-ZaloOtpProvider/1.0");
        })
        .AddStandardResilienceHandler();

        services.AddHttpClient<IOtpProvider, AnSinhSo.Infrastructure.Authentication.Providers.SmsOtpProvider>(client =>
        {
            client.Timeout = TimeSpan.FromSeconds(30);
            client.DefaultRequestHeaders.Add("User-Agent", "AnSinhSo-SmsOtpProvider/1.0");
        })
        .AddStandardResilienceHandler();
        services.AddScoped<IOtpDeliveryStrategy, AnSinhSo.Infrastructure.Authentication.Providers.OtpDeliveryStrategy>();

        // Audit Service (AD #119, AD #132)
        services.AddSingleton<AnSinhSo.Application.Abstractions.Audit.IAuditService, LoggerAuditService>();

        // Seeding
        services.Configure<AnSinhSo.Infrastructure.Persistence.Seeding.SeedAdminOptions>(configuration.GetSection(AnSinhSo.Infrastructure.Persistence.Seeding.SeedAdminOptions.SectionName));
        services.AddScoped<AnSinhSo.Infrastructure.Persistence.Seeding.IDataSeeder, AnSinhSo.Infrastructure.Persistence.Seeding.PermissionGroupSeeder>();
        services.AddScoped<AnSinhSo.Infrastructure.Persistence.Seeding.IDataSeeder, AnSinhSo.Infrastructure.Persistence.Seeding.PermissionSeeder>();
        services.AddScoped<AnSinhSo.Infrastructure.Persistence.Seeding.IDataSeeder, AnSinhSo.Infrastructure.Persistence.Seeding.RoleSeeder>();
        services.AddScoped<AnSinhSo.Infrastructure.Persistence.Seeding.IDataSeeder, AnSinhSo.Infrastructure.Persistence.Seeding.RolePermissionSeeder>();
        services.AddScoped<AnSinhSo.Infrastructure.Persistence.Seeding.IDataSeeder, AnSinhSo.Infrastructure.Persistence.Seeding.UserSeeder>();
        services.AddScoped<AnSinhSo.Infrastructure.Persistence.Seeding.IDataSeeder, AnSinhSo.Infrastructure.Persistence.Seeding.UserRoleSeeder>();

        // Demo Data Seeder
        services.Configure<AnSinhSo.Infrastructure.Persistence.Seeding.Demo.DemoDataOptions>(configuration.GetSection(AnSinhSo.Infrastructure.Persistence.Seeding.Demo.DemoDataOptions.SectionName));
        services.AddScoped<AnSinhSo.Infrastructure.Persistence.Seeding.IDataSeeder, AnSinhSo.Infrastructure.Persistence.Seeding.Demo.EnterpriseDemoSeeder>();
        services.AddScoped<AnSinhSo.Infrastructure.Persistence.Seeding.SeedRunner>();

        return services;
    }
}
