using EmployeeManagement.Database.Infrastructure;
using EmployeeManagement.Entities;

namespace EmployeeManagement.Database.Repositories
{
    public interface ITasksRepository : IRepository<Tasks>
    {
    }

    public class TasksRepository : Repository<Tasks>, ITasksRepository
    {
        public TasksRepository(EmployeeDbContext context) : base(context)
        {
        }
    }
}
