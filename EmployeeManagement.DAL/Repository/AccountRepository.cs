using EmployeeManagement.DAL.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.DAL.Repository
{
    public class AccountRepository
    {
        private readonly DataBaseContest _context;
        public AccountRepository( DataBaseContest contest)
        {
            _context = contest;
        }

        public async Task<List<User>> GetAllUsers()
        {
            return  _context.Users.ToList();
        }
    }
}
