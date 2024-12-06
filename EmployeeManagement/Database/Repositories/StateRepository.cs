using EmployeeManagement.Database.Infrastructure;
using EmployeeManagement.Entities;

namespace EmployeeManagement.Database.Repositories
{
    public interface IStateRepository : IRepository<State>
    {
    }

    public class StateRepository : Repository<State>, IStateRepository
    {
        public StateRepository(EmployeeDbContext context) : base(context)
        {
        }
    }
}
