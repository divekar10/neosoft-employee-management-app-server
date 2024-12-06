using FluentValidation.Results;
using Utilities.Content;

namespace EmployeeManagement.Infrastructure.Model
{
    public class ValidationFailureResponse
    {
        public IEnumerable<string> Errors { get; set; } = [];
    }

    public static class ValidationFailureMapper
    {
        public static ValidationFailureResponse ToResponse(this IEnumerable<ValidationFailure> failure)
        {
            return new ValidationFailureResponse
            {
                Errors = failure.Select(x => ContentLoader.ReturnLanguageData(x.ErrorMessage, ""))
            };
        }
    }
}
