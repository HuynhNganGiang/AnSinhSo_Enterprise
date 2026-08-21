using System;
using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Application.Abstractions.Authentication;
using AnSinhSo.Domain.Aggregates.SecurityAggregate;
using AnSinhSo.Domain.Aggregates.SecurityAggregate.Enumerations;
using AnSinhSo.Domain.Interfaces;

namespace AnSinhSo.Application.Authentication.Services;

public sealed class SecurityAuditService : ISecurityAuditService
{
    private readonly ISecurityRepository _securityRepository;
    private readonly IUnitOfWork _unitOfWork;

    public SecurityAuditService(ISecurityRepository securityRepository, IUnitOfWork unitOfWork)
    {
        _securityRepository = securityRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task LogSuccessAsync(Guid userId, string ipAddress, string userAgent, string deviceName, string timeZone, CancellationToken cancellationToken = default)
    {
        var loginHistory = LoginHistory.RecordLogin(
            userId: userId,
            ipAddress: ipAddress,
            deviceName: deviceName,
            location: timeZone
        );

        var auditLogin = AuditLogin.CreateSuccess(
            userId: userId,
            ipAddress: ipAddress,
            userAgent: userAgent,
            timeZone: timeZone
        );

        _securityRepository.AddLoginHistory(loginHistory);
        _securityRepository.AddAuditLogin(auditLogin);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task LogFailureAsync(Guid userId, string ipAddress, string userAgent, string timeZone, string reason, CancellationToken cancellationToken = default)
    {
        var auditLogin = AuditLogin.CreateFailure(
            userId: userId,
            ipAddress: ipAddress,
            userAgent: userAgent,
            timeZone: timeZone,
            failureReason: reason
        );

        var securityLog = SecurityLog.Create(
            userId: userId,
            eventType: SecurityEventType.LOGIN_FAILED,
            details: $"Failed login attempt. Reason: {reason}",
            ipAddress: ipAddress
        );

        _securityRepository.AddAuditLogin(auditLogin);
        _securityRepository.AddSecurityLog(securityLog);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task LogRateLimitExceededAsync(string target, string ipAddress, string deviceFingerprint, string reason, CancellationToken cancellationToken = default)
    {
        var securityLog = SecurityLog.Create(
            userId: Guid.Empty, // Unauthenticated user context
            eventType: SecurityEventType.OTP_FAILED,
            details: $"OTP Spam Blocked. Target: {target}, Device: {deviceFingerprint}. Reason: {reason}",
            ipAddress: ipAddress
        );

        _securityRepository.AddSecurityLog(securityLog);
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
