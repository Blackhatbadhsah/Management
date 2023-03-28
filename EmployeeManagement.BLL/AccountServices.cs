using EmployeeManagement.DAL;
using EmployeeManagement.DAL.Models;
using EmployeeManagement.DAL.Repository;
using EmployeeManagement.Shared;
using EmployeeManagement.Shared.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace EmployeeManagement.BLL
{
    public class AccountServices
    {
        private readonly IConfiguration _config;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly AccountRepository _context;
        public AccountServices(UserManager<User> userManager, RoleManager<Role> roleManager, IConfiguration configuration, AccountRepository contest)
        {
            _config = configuration;
            _userManager = userManager;
            _roleManager = roleManager;
            _context = contest;
        }

        public async Task<List<RegisterModel>> GetAllUsersAsync()
        {
            List<RegisterModel> users = new List<RegisterModel>();
           var data = await  _context.GetAllUsers();
            foreach (var user in data)
            {
                users.Add(new RegisterModel
                {
                    Name = user.NormalizedUserName,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    MiddleName = user.MiddleName,
                    DateOfBirth = user.DateOfBirth,
                    DateOfJoining = user.DateOfJoining,
                    DateOfMarriage = user.DateOfMarriage,
                    EmployeeType = user.EmployeeType,
                    Department= user.Department,
                    Email = user.Email,
                    UserName = user.UserName,
                    Role = Enum.Parse<Roles>(_userManager.GetRolesAsync(user).Result.FirstOrDefault()),
                }); ;
                
            }
            return users;
        }

        public async Task<string> LoginAsync(LoginModel model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user == null)
            {
                user = await _userManager.FindByEmailAsync(model.UserName);
            }
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var userRoles = await _userManager.GetRolesAsync(user);

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Email,user.Email),
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                var token = GetToken(authClaims);


                return new JwtSecurityTokenHandler().WriteToken(token);
            }

            throw new UnauthorizedAccessException();
        }


        public async Task<string> RegisterAsync(RegisterModel model)
        {

            var user = new User
            {
                UserName = model.UserName,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                MiddleName = model.MiddleName,
                DateOfBirth = model.DateOfBirth,
                DateOfJoining = model.DateOfJoining,
                DateOfMarriage = model.DateOfMarriage??null,
                Department = model.Department,
                EmployeeType = model.EmployeeType,
            };

            if (model.Password == model.ConfirmPassword)
            {
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    result = await _userManager.AddToRoleAsync(user, model.Role.ToString());
                    var Login = new LoginModel()
                    {
                        UserName = model.UserName,
                        Password = model.Password,
                    };
                    return await LoginAsync(Login);

                }
            }
            else
            {
                throw new Exception("Passwords does not Match");
            }
            throw new Exception("Something Went Wrong ! ");
        }

        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"] ?? ""));

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: "*",
                expires: DateTime.Now.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return token;
        }

        public async Task<string> EditUSerASync(RegisterModel model)
        {

            var user = new User
            {
                UserName = model.UserName,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                MiddleName = model.MiddleName,
                DateOfBirth = model.DateOfBirth,
                DateOfJoining = model.DateOfJoining,
                DateOfMarriage = model.DateOfMarriage ?? null,
                Department = model.Department,
                EmployeeType = model.EmployeeType,
            };

            if (model.Password == model.ConfirmPassword)
            {
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    result = await _userManager.AddToRoleAsync(user, model.Role.ToString());
                    var Login = new LoginModel()
                    {
                        UserName = model.UserName,
                        Password = model.Password,
                    };
                    return await LoginAsync(Login);

                }
            }
            else
            {
                throw new Exception("Passwords does not Match");
            }
            throw new Exception("Something Went Wrong ! ");
        }
    }
}