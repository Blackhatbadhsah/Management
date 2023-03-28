using EmployeeManagement.Shared.Enums;
using Microsoft.AspNetCore.Authorization;

namespace EmployeeManagement.Client.Services
{
    public class RoleBasedAuthorizationPolicyProvider : IAuthorizationPolicyProvider
    {

        public async Task<AuthorizationPolicy> GetDefaultPolicyAsync()
        {
            return new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();
        }

        public async Task<AuthorizationPolicy> GetFallbackPolicyAsync()
        {
            return new AuthorizationPolicyBuilder()
               .RequireAuthenticatedUser()
               .Build();
        }

        public async Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
        {
            switch (policyName)
            {
                case "SuperAdmin":
                    return new AuthorizationPolicyBuilder()
                        .RequireAuthenticatedUser()
                        .RequireRole("SuperAdmin")
                        .Build();
                   
                case "Director":
                    return new AuthorizationPolicyBuilder()
                        .RequireAuthenticatedUser()
                        .RequireRole("SuperAdmin","Director")
                        .Build();
                    
                case "Admin":
                    return new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .RequireRole("SuperAdmin","Director","Admin")
                    .Build();
                    
                case "Supervisor":
                    return new AuthorizationPolicyBuilder()
                        .RequireAuthenticatedUser()
                        .RequireRole("SuperAdmin","Director","Admin","Supervisor")
                        .Build();
                    
                case "SDE3":
                    return new AuthorizationPolicyBuilder()
                        .RequireAuthenticatedUser()
                        .RequireRole("SuperAdmin","Director",",Admin","Supervisor","SDE3")
                        .Build();
                    
                case "SDE2":
                    return new AuthorizationPolicyBuilder()
                        .RequireAuthenticatedUser()
                        .RequireRole("SuperAdmin","Director","Admin","Supervisor","SDE3","SDE2")
                        .Build();
                    
                case "SDE1":
                    return new AuthorizationPolicyBuilder()
                        .RequireAuthenticatedUser()
                        .RequireRole("SuperAdmin","Director","Admin","Supervisor","SDE3","SDE2","SDE1")
                        .Build();
                    
                case "HR":
                    return new AuthorizationPolicyBuilder()
                        .RequireAuthenticatedUser()
                        .RequireRole("SuperAdmin","Director","Admin","Supervisor","SDE3","SDE2","SDE1","HR")
                        .Build();
                    
                case "SupportStaff":
                    return new AuthorizationPolicyBuilder()
                        .RequireAuthenticatedUser()
                        .RequireRole("SuperAdmin","Director","Admin","Supervisor","SDE3","SDE2","SDE1","HR","SupportStaff")
                        .Build();
                    
                case "Guest":
                    return new AuthorizationPolicyBuilder()
                        .RequireAuthenticatedUser()
                        .RequireRole("SuperAdmin","Director","Admin","Supervisor","SDE3","SDE2","SDE1","HR","SupportStaff","Guest")
                        .Build();
                    
                default:
                    return new AuthorizationPolicyBuilder()
                       .RequireAuthenticatedUser()
                       .Build();
            }
        }
    }
    
}
