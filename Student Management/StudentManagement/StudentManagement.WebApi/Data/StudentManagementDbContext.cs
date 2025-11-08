using Microsoft.EntityFrameworkCore;
using StudentManagement.WebApi.Data.Entities;

namespace StudentManagement.WebApi.Data
{
    public class StudentManagementDbContext : DbContext
    {
        public StudentManagementDbContext(DbContextOptions<StudentManagementDbContext> options) : base(options) 
        {
            
        }

        public DbSet<StudentEntity> Students { get; set; }
    }
}
