using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Shared.EndPoints
{
    public static class EndPoints
    {
        public static string LOGIN { get; set; } = "api/account/login";
        public static string REGISTER { get; set; } = "api/account/register";
        public static string GETUSERS { get; set; } = "api/account/listusers";
    }
}
