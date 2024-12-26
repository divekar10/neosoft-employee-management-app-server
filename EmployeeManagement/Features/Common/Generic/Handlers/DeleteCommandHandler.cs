using EmployeeManagement.Database;
using EmployeeManagement.Entities.BaseEntity;
using EmployeeManagement.Features.Common.Generic.Commands;
using EmployeeManagement.Shared.Enum;
using EmployeeManagement.Shared.Result;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Utilities.Content;

namespace EmployeeManagement.Features.Common.Generic.Handlers
{
    public class DeleteCommandHandler<T> : IRequestHandler<DeleteCommand<T>, Result<bool>>
        where T : class, IEntity
    {
        private readonly EmployeeDbContext _context;
        public DeleteCommandHandler(EmployeeDbContext context) => _context = context;

        public async Task<Result<bool>> Handle(DeleteCommand<T> request, CancellationToken cancellationToken)
        {
            if (request is null)
            {
                return Result.Failure<bool>(new(ErrorType.Validation, ContentLoader.ReturnLanguageData("EMP103")));
            }

            var entity = await _context.Set<T>().FindAsync(new object[] { request.Id }, cancellationToken);
            if (entity == null) return false;
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
