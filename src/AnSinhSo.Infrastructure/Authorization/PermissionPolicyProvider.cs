using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace AnSinhSo.Infrastructure.Authorization;

public class PermissionPolicyProvider : DefaultAuthorizationPolicyProvider
{
    private const string PolicyPrefix = "Permission:";

    public PermissionPolicyProvider(IOptions<AuthorizationOptions> options) : base(options)
    {
    }

    public override async Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    {
        if (policyName.StartsWith(PolicyPrefix, StringComparison.OrdinalIgnoreCase))
        {
            var permission = policyName.Substring(PolicyPrefix.Length);
            
            var policy = new AuthorizationPolicyBuilder();
            policy.AddRequirements(new PermissionRequirement(permission));
            
            return policy.Build();
        }

        return await base.GetPolicyAsync(policyName);
    }
}
