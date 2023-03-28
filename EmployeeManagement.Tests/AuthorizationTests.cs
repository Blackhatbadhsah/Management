using EmployeeManagement.DAL;
using EmployeeManagement.DAL.Models;
using EmployeeManagement.Shared.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace EmployeeManagement.Tests
{
    public class AuthorizationTests: IClassFixture<InjectionFixture>
    {
        private readonly UserManager<User> userManager;
        private readonly RoleManager<Role> roleManager;
        public AuthorizationTests(InjectionFixture f)
        {
            var s = f.ServiceProvider;

            roleManager = s.GetRequiredService<RoleManager<Role>>();
            userManager = s.GetRequiredService<UserManager<User>>();
        }

        [Fact]
        public async void CreateUSerTest()
        {


            var role = new Role
            {
                Name = Roles.SuperAdmin.ToString()
            };

            await roleManager.CreateAsync(role);

            var user = new User
            {
                UserName = "vermasj",
                Email = "sanjayv@appinfoinc.com",
                FirstName = "Sanjay",
                LastName = "Verma",
                DateOfBirth = new DateTime(1990,03,04),
                DateOfJoining = new DateTime(2022,10,17),
                Department=Department.Backend,
                EmployeeType=EmployeeType.SDE,

            };

            var result = await userManager.CreateAsync(user, "User*123");

            if (result.Succeeded)
            {
                result = await userManager.AddToRoleAsync(user, Roles.SuperAdmin.ToString());
                Assert.True(result.Succeeded);
            }
            else
            {
                Assert.True(false);
            }
        }
    }
}