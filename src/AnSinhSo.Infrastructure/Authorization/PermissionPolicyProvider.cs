using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace AnSinhSo.Infrastructure.Authorization;

public class PermissionPolicyProvider : DefaultAuthorizationPolicyProvider
{
    public PermissionPolicyProvider(IOptions<AuthorizationOptions> options) : base(options)
    {
    }

    public override async Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    {
        // Try to get from base first (for Default policies etc)
        var policy = await base.GetPolicyAsync(policyName);
        if (policy != null)
        {
            return policy;
        }

        // If not found, treat it as a permission
        var policyBuilder = new AuthorizationPolicyBuilder();
        policyBuilder.AddRequirements(new PermissionRequirement(policyName));
        return policyBuilder.Build();
    }
}
