using EmployeeManagement.Entities.BaseEntity;
using EmployeeManagement.Features.Common.Generic.Commands;
using EmployeeManagement.Features.Common.Generic.Queries;
using EmployeeManagement.Infrastructure.Extensions;
using MediatR;

namespace EmployeeManagement.Features.Common.Generic.Endpoints
{
    public static class GenericEndpoints
    {
        public static void MapGenericCrudEndpoints<T>(this IEndpointRouteBuilder endpoints)
        where T : class, IEntity
        {
            var entityName = typeof(T).Name.ToLower();
            var group = endpoints.MapGroup(entityName);

            //Create resource
            group.MapPost("", async (T entity, ISender sender) =>
            {
                var result = await sender.Send(new CreateCommand<T>(entity));

                if (result.IsSuccess)
                {
                    return result.ToSuccess();
                }
                else
                {
                    return result.ToProblemDetails();
                }
            }).WithTags(entityName);

            //update resource
            group.MapPut($"/{{id}}", async (int id, T entity, ISender sender) =>
            {
                if (id != entity.Id)
                {
                    return Results.BadRequest();
                }

                var result = await sender.Send(new UpdateCommand<T>(entity));

                if (result.IsSuccess)
                {
                    return result.ToSuccess();
                }
                else
                {
                    return result.ToProblemDetails();
                }
            }).WithTags(entityName);

            //Delete resource
            group.MapDelete($"/{{id}}", async (int id, ISender sender) =>
            {
                var success = await sender.Send(new DeleteCommand<T>(id));

                if (success.IsSuccess)
                {
                    return success.ToSuccess();
                }
                else
                {
                    return success.ToProblemDetails();
                }
            }).WithTags(entityName);

            //Get resource by Id
            group.MapGet($"/{{id}}", async (int id, ISender sender) =>
            {
                var result = await sender.Send(new GetByIdQuery<T>(id));

                if (result.IsSuccess)
                {
                    return result.ToSuccess();
                }
                else
                {
                    return result.ToProblemDetails();
                }
            }).WithTags(entityName);

            // Get all resources
            group.MapGet($"", async (ISender sender) =>
            {
                var result = await sender.Send(new GetAllQuery<T>());
                if (result.IsSuccess)
                {
                    return result.ToSuccess();
                }
                else
                {
                    return result.ToProblemDetails();
                }
            }).WithTags(entityName);
        }
    }
}
