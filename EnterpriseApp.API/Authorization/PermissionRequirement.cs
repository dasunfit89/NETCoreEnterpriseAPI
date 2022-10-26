using Microsoft.AspNetCore.Authorization;

namespace EnterpriseApp.API.Authorization
{
    public class PermissionRequirement : IAuthorizationRequirement
    {
        public string Permissons { get; private set; }

        public PermissionRequirement(string permissions)
        {
            Permissons = permissions;
        }
    }
}
