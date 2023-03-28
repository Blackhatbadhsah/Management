using EmployeeManagement.BLL;
using EmployeeManagement.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.Server.Controllers
{
    [EnableCors]
    [Route("api/[Controller]")]
    public class AccountController : ControllerBase
    {
        private readonly AccountServices _services;
        public AccountController(AccountServices services)
        {
            _services= services;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            try
            {
               return  Ok(new AuthResponse(){ Token = await _services.LoginAsync(model) });
            }
            catch(Exception ex)
            {
                return Unauthorized(ex.Message);
            }
        }
        [Authorize(Roles = "SuperAdmin,Director,Admin,Supervisor")]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            try
            {
                return Ok(new AuthResponse() { Token = await _services.RegisterAsync(model) });
            }
            catch (Exception ex)
            {
                return Unauthorized(ex.Message);
            }
        }
        [Authorize(Roles = "SuperAdmin,Director,Admin,Supervisor,HR,SDE3")]
        [HttpGet("listusers")]
        public async Task<IActionResult> listUsers()
        {
            try
            {
                List<RegisterModel> Users = await _services.GetAllUsersAsync();
                return Ok(Users);
            }
            catch (Exception ex)
            {
                return Unauthorized(ex.StackTrace);
            }
        }
    }
}
