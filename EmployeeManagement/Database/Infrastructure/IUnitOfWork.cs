using EmployeeManagement.Database.Repositories;
using Microsoft.EntityFrameworkCore.Storage;

namespace EmployeeManagement.Database.Infrastructure
{
    public interface IUnitOfWork
    {
        IEmployeeRepository EmployeeRepository { get; }
        ICountryRepository CountryRepository { get; }
        IStateRepository StateRepository { get; }
        ICityRepository CityRepository { get; }
        ITasksRepository TasksRepository { get; }
        public Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default);
        Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);
        Task CommitTransactionAsync(CancellationToken cancellationToken = default);
        Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
    }
}
