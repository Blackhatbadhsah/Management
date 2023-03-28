using EmployeeManagement.Shared.Enums;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.DAL.Models
{
    public class User:IdentityUser<Guid>
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string? MiddleName { get; set; }
        public string? Address { get; set; } = null!;
        public string? City { get; set; } = null!;
        public string? Region { get; set; } = null!;
        public string? PostalCode { get; set; } = null!;
        public string? ProfileImagePath { get; set; } = null!;
        public string? Country { get; set; } = null!;
        public DateTime DateOfBirth { get; set; }
        public DateTime? DateOfMarriage { get; set; }
        public DateTime DateOfJoining { get; set; }
        public Department Department { get; set; }
        public EmployeeType EmployeeType { get; set; }

    }
}
