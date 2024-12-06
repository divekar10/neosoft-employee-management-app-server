using EmployeeManagement.Database.Infrastructure;
using EmployeeManagement.Entities;

namespace EmployeeManagement.Database.Repositories
{
    public interface ICityRepository : IRepository<City>
    {
    }

    public class CityRepository : Repository<City>, ICityRepository
    {
        public CityRepository(EmployeeDbContext context) : base(context)
        {
        }
    }
}
