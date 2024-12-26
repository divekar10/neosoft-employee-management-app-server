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
    public class CreateCommandHandler<T> : IRequestHandler<CreateCommand<T>, Result<T>>
        where T : class, IEntity
    {
        private readonly EmployeeDbContext _context;
        public CreateCommandHandler(EmployeeDbContext context) => _context = context;

        public async Task<Result<T>> Handle(CreateCommand<T> request, CancellationToken cancellationToken)
        {
            if (request is null)
            {
                return Result.Failure<T>(new(ErrorType.Validation, ContentLoader.ReturnLanguageData("EMP103")));
            }

            var entity = request.Entity;
            _context.Set<T>().Add(entity);
            await _context.SaveChangesAsync(cancellationToken);
            return Result.Success(entity);
        }
    }
}
