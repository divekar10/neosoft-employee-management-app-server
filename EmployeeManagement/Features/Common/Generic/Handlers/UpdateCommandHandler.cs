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
    public class UpdateCommandHandler<T> : IRequestHandler<UpdateCommand<T>, Result<T>>
        where T : class
    {
        private readonly EmployeeDbContext _context;
        public UpdateCommandHandler(EmployeeDbContext context) => _context = context;

        public async Task<Result<T>> Handle(UpdateCommand<T> request, CancellationToken cancellationToken)
        {
            if (request is null)
            {
                return Result.Failure<T>(new(ErrorType.Validation, ContentLoader.ReturnLanguageData("EMP103")));
            }

            _context.Set<T>().Update(request.Entity);
            await _context.SaveChangesAsync(cancellationToken);
            return request.Entity;
        }
    }
}
