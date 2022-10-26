using Microsoft.AspNetCore.Authorization.Infrastructure;
using System.Collections.Generic;

namespace EnterpriseApp.API.Authorization
{
    public class RoleClaimsAuthorizationRequirement : ClaimsAuthorizationRequirement
    {
        public RoleClaimsAuthorizationRequirement(
          string claimType,
          IEnumerable<string> allowedValues) :
          base(claimType, allowedValues)
        {
        }
    }
}
