using EmployeeManagement.Entities;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Database
{
    public class EmployeeDbContext : DbContext
    {
        public EmployeeDbContext(DbContextOptions<EmployeeDbContext> options) : base(options) { }

        public required DbSet<Employee> Employee { get; set; }
        public required DbSet<Country> Country { get; set; }
        public required DbSet<State> State { get; set; }
        public required DbSet<City> City { get; set; }
        public required DbSet<Department> Department { get; set; }
    }
}
