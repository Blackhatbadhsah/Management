using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace EmployeeManagement.Client.Services
{
    public class AuthorizationService : IAuthorizationService
    {
        public async Task<AuthorizationResult> AuthorizeAsync(ClaimsPrincipal user, object? resource, IEnumerable<IAuthorizationRequirement> requirements)
        {
            if (user == null||user.Identities.Count()==0 || !user.Identity.IsAuthenticated)
            {
                return AuthorizationResult.Failed();
            }

            // Get the user's email address and roles from the claims in the JWT token
            string email = user.FindFirstValue(ClaimTypes.Email);
            IList<string> roles = user.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList();

            // Check if the user has any of the required roles
            foreach (IAuthorizationRequirement requirement in requirements)
            {
                if (requirement is RolesAuthorizationRequirement rolesRequirement)
                {
                    if (rolesRequirement.AllowedRoles.Any(r => roles.Contains(r)))
                    {
                        return AuthorizationResult.Success();
                    }
                }

            }

            return AuthorizationResult.Failed();
        }

        public Task<AuthorizationResult> AuthorizeAsync(ClaimsPrincipal user, object? resource, string policyName)
        {
            // TODO: Implement policy-based authorization
            throw new NotImplementedException();
        }
    }

}
