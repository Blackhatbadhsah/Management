using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.Shared
{
    public class LoginModel
    {
        [Required]
        public string UserName { get; set; } = null!;
        [Required]
        [PasswordPropertyText]
        public string Password { get; set; } = null!;
    }
}