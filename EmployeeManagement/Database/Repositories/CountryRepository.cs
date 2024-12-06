using EmployeeManagement.Database.Infrastructure;
using EmployeeManagement.Entities;

namespace EmployeeManagement.Database.Repositories
{
    public interface ICountryRepository : IRepository<Country>
    {
    }

    public class CountryRepository : Repository<Country>, ICountryRepository
    {
        public CountryRepository(EmployeeDbContext context) : base(context)
        {
        }
    }
}
