using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using EnterpriseApp.API.Core.Authorizations;
using EnterpriseApp.API.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace EnterpriseApp.API.Authorization
{
    public class PermissionHandler : IAuthorizationHandler
    {
        private readonly ILogger<PermissionHandler> _logger;
        private readonly IUserService _userManager;

        public PermissionHandler(ILogger<PermissionHandler> logger, IUserService userManager)
        {
            _logger = logger;
            _userManager = userManager;
        }

        public Task HandleAsync(AuthorizationHandlerContext context)
        {
            if (!context.User.Identity.IsAuthenticated)
            {
                _logger.LogError($"Permission denied - not authenticated");
                return Task.CompletedTask;
            }

            var pendingRequirements = context.PendingRequirements.ToList();
            Claim emailClaim = context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);

            if (emailClaim == null)
            {
                _logger.LogError($"Permission denied - no email claim");
                return Task.CompletedTask;
            }

            foreach (var requirement in pendingRequirements)
            {
                if (requirement is RoleClaimsAuthorizationRequirement roleClaimsRequirement)
                {
                    var claimType = roleClaimsRequirement.ClaimType;
                    var claimValue = roleClaimsRequirement.AllowedValues.FirstOrDefault();

                    List<string> userRoleClaims = _userManager.GetUserRoleClaims(emailClaim.Value).Result;

                    string roleClaim = userRoleClaims?.FirstOrDefault(x => x == (claimValue) || x == Permissions.SuperUser);

                    if (roleClaim == null)
                    {
                        _logger.LogError($"Permission denied - required permission " +
                          $"{claimType} not granted for email {emailClaim.Value}");
                        continue;
                    }
                    else
                    {
                        context.Succeed(requirement);
                    }
                }
            }

            return Task.CompletedTask;
        }
    }
}
