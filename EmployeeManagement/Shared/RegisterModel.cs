using EmployeeManagement.Shared.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Shared
{
    public class RegisterModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string? MiddleName { get; set; }
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        //public string PhoneNumber { get; set; } = null!;
        //public string Address { get; set; } = null!;
        //public string City { get; set; } = null!;
        //public string Region { get; set; } = null!;
        //public string PostalCode { get; set; } = null!;
        //public string ProfileImagePath { get; set; } = null!;
        //public string Country { get; set; } = null!;
        public DateTime DateOfBirth { get; set; } = DateTime.UtcNow;
        public DateTime? DateOfMarriage { get; set; } = null!;
        public DateTime DateOfJoining { get; set; } = DateTime.UtcNow;
        public Department Department { get; set; } 
        public EmployeeType EmployeeType { get; set; }
        [PasswordPropertyText]
        public string Password { get; set; } = null!;
        [PasswordPropertyText]
        public string ConfirmPassword { get; set; }= null!;
        public Roles Role{ get; set; }
    }
}
