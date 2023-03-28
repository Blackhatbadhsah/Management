using EmployeeManagement.DAL.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.DAL
{
    public class DataBaseContest :IdentityDbContext  <User,Role,Guid>
    {
        public DataBaseContest(DbContextOptions<DataBaseContest> options)
    : base(options)
        {
        }
       
    }
}