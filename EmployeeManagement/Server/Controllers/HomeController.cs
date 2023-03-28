using EmployeeManagement.Shared.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.Server.Controllers
{
    [EnableCors]
    [Authorize(Roles = "SuperAdmin,Admin")]
    [Route("api/[Controller]")]
    public class HomeController:ControllerBase
    {
        public HomeController()
        {

        }

        [HttpGet("dashboard/{Id}")]
        public IActionResult GetUserDashboard([FromRoute] string Id)
        {
            return Ok(new { Id = Id });
        }
    }
}
