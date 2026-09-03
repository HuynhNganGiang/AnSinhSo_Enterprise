using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Domain.Aggregates.CitizenAggregate;
using AnSinhSo.Domain.Aggregates.CitizenAggregate.Enumerations;
using AnSinhSo.Domain.Aggregates.HouseholdAggregate;
using AnSinhSo.Domain.Aggregates.PaymentAggregate;
using AnSinhSo.Domain.Aggregates.PolicyAggregate;
using AnSinhSo.Domain.Aggregates.WelfareGroupAggregate;
using AnSinhSo.Domain.Aggregates.WelfareProgramAggregate;
using AnSinhSo.Domain.Aggregates.WelfareCaseAggregate;
using AnSinhSo.Domain.Aggregates.RelationshipTypeAggregate;
using AnSinhSo.Domain.Aggregates.NotificationAggregate;
using AnSinhSo.Domain.Aggregates.AiRecommendationAggregate;
using AnSinhSo.Domain.Aggregates.SecurityAggregate;
using AnSinhSo.Domain.Aggregates.SecurityAggregate.ValueObjects;
using AnSinhSo.Domain.Aggregates.ZaloUserAggregate;
using AnSinhSo.Domain.ValueObjects;
using AnSinhSo.Domain.Aggregates.UserAggregate;
using AnSinhSo.Domain.Aggregates.CitizenIdentityAggregate;
using AnSinhSo.Domain.Aggregates.PaymentPointAggregate;
using AnSinhSo.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AnSinhSo.Infrastructure.Persistence.Seeding.Demo;

public class EnterpriseDemoSeeder : IDataSeeder
{
    private readonly DemoDataOptions _options;
    private readonly ILogger<EnterpriseDemoSeeder> _logger;

    public int Order => 100; // Run last

    public EnterpriseDemoSeeder(IOptions<DemoDataOptions> options, ILogger<EnterpriseDemoSeeder> logger)
    {
        _options = options.Value;
        _logger = logger;
    }

    public async Task SeedAsync(AnSinhSoDbContext context, IServiceProvider serviceProvider, CancellationToken cancellationToken = default)
    {
        if (!_options.Enabled)
        {
            _logger.LogInformation("Demo data seeding is disabled.");
            return;
        }

        _logger.LogInformation("--- STARTING ENTERPRISE DEMO DATA SEEDING ---");

        // 1. Foundation: RelationshipTypes
        var relTypes = await SeedRelationshipTypesAsync(context, cancellationToken);

        // 2. Metadata: Policies, WelfareGroups, WelfarePrograms
        var policy = await SeedPolicyAsync(context, cancellationToken);
        var welfareGroup = await SeedWelfareGroupAsync(context, cancellationToken);
        var welfareProgram = await SeedWelfareProgramAsync(context, policy, cancellationToken);

        // 3. PaymentPoints
        var paymentPoint = await SeedPaymentPointAsync(context, cancellationToken);

        // 4. Core Business Chain (Household -> Citizen -> WelfareCase -> Payment -> Notification -> Audit -> AI Recommendation)
        await SeedBusinessChainsAsync(context, relTypes, welfareGroup, welfareProgram, paymentPoint, cancellationToken);

        _logger.LogInformation("--- COMPLETED ENTERPRISE DEMO DATA SEEDING ---");

        DemoDataReportGenerator.GenerateReport(_options);
    }

    private async Task<List<RelationshipType>> SeedRelationshipTypesAsync(AnSinhSoDbContext context, CancellationToken ct)
    {
        var existing = await context.RelationshipTypes.ToListAsync(ct);
        if (existing.Any()) return existing;

        var types = new List<RelationshipType>
        {
            RelationshipType.Create(new RelationshipTypeId(Guid.NewGuid()), "Chủ hộ", "CHU_HO", "Chủ hộ gia đình"),
            RelationshipType.Create(new RelationshipTypeId(Guid.NewGuid()), "Vợ", "VO", "Vợ của chủ hộ"),
            RelationshipType.Create(new RelationshipTypeId(Guid.NewGuid()), "Chồng", "CHONG", "Chồng của chủ hộ"),
            RelationshipType.Create(new RelationshipTypeId(Guid.NewGuid()), "Con đẻ", "CON_DE", "Con đẻ"),
            RelationshipType.Create(new RelationshipTypeId(Guid.NewGuid()), "Cha mẹ", "CHA_ME", "Cha hoặc mẹ")
        };

        await context.RelationshipTypes.AddRangeAsync(types, ct);
        await context.SaveChangesAsync(ct);
        return types;
    }

    private async Task<Policy> SeedPolicyAsync(AnSinhSoDbContext context, CancellationToken ct)
    {
        var policy = await context.Policies.FirstOrDefaultAsync(ct);
        if (policy == null)
        {
            policy = Policy.Create(new PolicyId(Guid.NewGuid()), "NĐ 20/2021/NĐ-CP", "Nghị định quy định chính sách trợ giúp xã hội", Money.Create(500000, AnSinhSo.Domain.Enumerations.Currency.VND).Value).Value;
            await context.Policies.AddAsync(policy, ct);
            await context.SaveChangesAsync(ct);
        }
        return policy;
    }

    private async Task<WelfareGroup> SeedWelfareGroupAsync(AnSinhSoDbContext context, CancellationToken ct)
    {
        var group = await context.WelfareGroups.FirstOrDefaultAsync(ct);
        if (group == null)
        {
            group = WelfareGroup.Create(new WelfareGroupId(Guid.NewGuid()), "Người cao tuổi", "Nhóm người cao tuổi neo đơn").Value;
            await context.WelfareGroups.AddAsync(group, ct);
            await context.SaveChangesAsync(ct);
        }
        return group;
    }

    private async Task<WelfareProgram> SeedWelfareProgramAsync(AnSinhSoDbContext context, Policy policy, CancellationToken ct)
    {
        var program = await context.WelfarePrograms.FirstOrDefaultAsync(ct);
        if (program == null)
        {
            program = WelfareProgram.Create(new WelfareProgramId(Guid.NewGuid()), "HT_NCT", "Hỗ trợ NCT Hàng Tháng", "Chi trả hàng tháng cho người cao tuổi").Value;
            await context.WelfarePrograms.AddAsync(program, ct);
            await context.SaveChangesAsync(ct);
        }
        return program;
    }

    private async Task<PaymentPoint> SeedPaymentPointAsync(AnSinhSoDbContext context, CancellationToken ct)
    {
        var pp = await context.PaymentPoints.FirstOrDefaultAsync(ct);
        if (pp == null)
        {
            var location = Location.Create(11.21011269694565, 108.32172004484949).Value; // UBND xa Song Luy - Thon 2
            var ppAddress = Address.Create("Thôn 2", "Xã Sông Lũy", "", "Lâm Đồng", PostalCode.Create("77317").Value).Value;
            pp = PaymentPoint.Create(new PaymentPointId(Guid.NewGuid()), "PP01", "Điểm chi trả xã Sông Lũy", ppAddress, location).Value;
            await context.PaymentPoints.AddAsync(pp, ct);
            await context.SaveChangesAsync(ct);
        }
        return pp;
    }

    private async Task SeedBusinessChainsAsync(
        AnSinhSoDbContext context,
        List<RelationshipType> relTypes,
        WelfareGroup welfareGroup,
        WelfareProgram welfareProgram,
        PaymentPoint paymentPoint,
        CancellationToken ct)
    {
        if (await context.Households.AnyAsync(ct))
        {
            _logger.LogInformation("Business chains already seeded. Skipping...");
            return;
        }

        var chuHoRel = relTypes.First(x => x.Code == "CHU_HO");
        var chaMeRel = relTypes.First(x => x.Code == "CHA_ME");

        var citizens = new List<Citizen>();
        var households = new List<Household>();
        var welfareCases = new List<WelfareCase>();
        var payments = new List<Payment>();
        var notifications = new List<Notification>();
        var aiRecs = new List<AiRecommendation>();
        var auditLogins = new List<AuditLogin>();
        var securityLogs = new List<SecurityLog>();

        _logger.LogInformation($"Generating {_options.Households} households and business chains...");

        for (int i = 0; i < _options.Households; i++)
        {
            // 1. Household
            var householdId = new HouseholdId(Guid.NewGuid());
            var address = Address.Create(DemoDataGenerator.GenerateAddress(), "Xã Sông Lũy", "Bắc Bình", "Lâm Đồng", PostalCode.Create("77317").Value).Value;
            var householdCodeValue = new HouseholdCode($"HK-{DateTime.UtcNow.Year}-{1000 + i}");
            var hh = Household.Create(householdId, householdCodeValue, address).Value;
            households.Add(hh);

            // 2. Head of Household (Citizen)
            bool isMale = DemoDataGenerator.NextBool(0.6);
            var headNameParts = DemoDataGenerator.GenerateName(isMale).Split(' ');
            var fn = FullName.Create(headNameParts.Last(), headNameParts.Length > 2 ? headNameParts[1] : "", headNameParts.First()).Value;
            var cn = CitizenNumber.Create(DemoDataGenerator.GenerateCitizenId()).Value;
            var phone = PhoneNumber.Create(DemoDataGenerator.GeneratePhoneNumber()).Value;
            var email = Email.Create($"citizen{i}@demo.com").Value;

            var headAddress = Address.Create(DemoDataGenerator.GenerateAddress(), "Xã Sông Lũy", "Bắc Bình", "Lâm Đồng", PostalCode.Create("77317").Value).Value;
            var headCitizen = Citizen.Create(
                new CitizenId(Guid.NewGuid()),
                fn,
                cn,
                DemoDataGenerator.GenerateBirthDate(30, 60),
                isMale ? Gender.Male : Gender.Female,
                phone,
                headAddress,
                email).Value;

            citizens.Add(headCitizen);

            // Add member to household
            hh.AddMember(headCitizen.Id, chuHoRel.Id, true);

            // Randomly create an Elderly person (maybe head's parent)
            if (DemoDataGenerator.NextBool(0.3))
            {
                var elderlyNameParts = DemoDataGenerator.GenerateName(DemoDataGenerator.NextBool()).Split(' ');
                var elderlyFn = FullName.Create(elderlyNameParts.Last(), elderlyNameParts.Length > 2 ? elderlyNameParts[1] : "", elderlyNameParts.First()).Value;
                var elderlyCn = CitizenNumber.Create(DemoDataGenerator.GenerateCitizenId()).Value;
                var elderlyPhone = PhoneNumber.Create(DemoDataGenerator.GeneratePhoneNumber()).Value;
                var elderlyEmail = Email.Create($"elderly{i}@demo.com").Value;

                var elderlyAddress = Address.Create(DemoDataGenerator.GenerateAddress(), "Xã Sông Lũy", "Bắc Bình", "Lâm Đồng", PostalCode.Create("77317").Value).Value;
                var elderlyCitizen = Citizen.Create(
                    new CitizenId(Guid.NewGuid()),
                    elderlyFn,
                    elderlyCn,
                    DemoDataGenerator.GenerateBirthDate(65, 90), // NCT
                    DemoDataGenerator.NextBool() ? Gender.Male : Gender.Female,
                    elderlyPhone,
                    elderlyAddress,
                    elderlyEmail).Value;

                citizens.Add(elderlyCitizen);
                hh.AddMember(elderlyCitizen.Id, chaMeRel.Id, false);

                // 3. Welfare Case for Elderly
                var caseId = new WelfareCaseId(Guid.NewGuid());
                var citizenNameStr = $"{elderlyCitizen.FullName.LastName} {elderlyCitizen.FullName.FirstName}";
                var snapshot = CitizenSnapshot.Create(
                    elderlyCitizen.CitizenNumber.Value,
                    citizenNameStr,
                    elderlyCitizen.BirthDate,
                    elderlyCitizen.Gender.ToString(),
                    elderlyCitizen.Email?.Value,
                    elderlyCitizen.PhoneNumber?.Value ?? "",
                    hh.HouseholdCode.Value);

                var wCase = WelfareCase.Create(caseId, elderlyCitizen.Id, hh.Id, welfareProgram.Id, snapshot).Value;
                wCase.UpdateDetails("Hỗ trợ NCT thường xuyên", 500000m, DateTime.UtcNow, DateTime.UtcNow.AddYears(1));
                welfareCases.Add(wCase);

                // 4. Payments for this case
                var paymentId = new PaymentId(Guid.NewGuid());
                var payment = Payment.Create(
                    paymentId,
                    $"PAY-{DateTime.UtcNow.Ticks}-{i}",
                    elderlyCitizen.Id,
                    hh.Id,
                    wCase.Id,
                    500000m,
                    DateTime.UtcNow,
                    AnSinhSo.Domain.Aggregates.PaymentAggregate.PaymentMethod.Cash,
                    "Trợ cấp tháng này").Value;
                payment.Submit();
                payment.Approve();
                payment.StartProcessing();
                payment.Complete(DateTime.UtcNow);
                payments.Add(payment);

                // 5. Notification
                var notif = Notification.Create("Nhận tiền hỗ trợ NCT", $"Bạn đã được chi trả 500,000 VND tại {paymentPoint.Name}", elderlyCitizen.Id, NotificationChannel.InApp, NotificationPriority.Normal);
                notif.MarkAsRead(DateTime.UtcNow);
                notifications.Add(notif);

                // 6. Audit & Security Log (Citizen viewing notification)
                var auditLogin = AuditLogin.CreateSuccess(elderlyCitizen.Id.Value, "192.168.1.10", "Mozilla/5.0", "Asia/Ho_Chi_Minh");
                auditLogins.Add(auditLogin);
                var secLog = SecurityLog.Create(elderlyCitizen.Id.Value, AnSinhSo.Domain.Aggregates.SecurityAggregate.Enumerations.SecurityEventType.LOGIN_SUCCESS, "Citizen logged in to view payment notification", "192.168.1.10");
                securityLogs.Add(secLog);

                // 7. AI Recommendation
                var aiRec = AiRecommendation.Create($"AI-{DateTime.UtcNow.Ticks}-{i}", AiTargetType.Citizen, elderlyCitizen.Id.Value, "v1.0", AiCategory.Suggestion, 95, AiConfidence.High, "NCT có thể hưởng thêm BHYT", "Hệ thống phát hiện công dân có thể hưởng BHYT miễn phí", new List<AiReason>());
                aiRecs.Add(aiRec);
            }
        }

        // Batch insert to prevent memory overflow
        await BulkInsertAsync(context, citizens, ct);
        await BulkInsertAsync(context, households, ct);
        await BulkInsertAsync(context, welfareCases, ct);
        await BulkInsertAsync(context, payments, ct);
        await BulkInsertAsync(context, notifications, ct);
        await BulkInsertAsync(context, auditLogins, ct);
        await BulkInsertAsync(context, securityLogs, ct);
        await BulkInsertAsync(context, aiRecs, ct);

        await context.SaveChangesAsync(ct);
    }

    private async Task BulkInsertAsync<T>(AnSinhSoDbContext context, List<T> entities, CancellationToken ct) where T : class
    {
        int batchSize = 1000;
        for (int i = 0; i < entities.Count; i += batchSize)
        {
            var batch = entities.Skip(i).Take(batchSize).ToList();
            await context.Set<T>().AddRangeAsync(batch, ct);
            await context.SaveChangesAsync(ct);
            // context.ChangeTracker.Clear(); // Can be used if memory pressure is high
        }
    }
}
