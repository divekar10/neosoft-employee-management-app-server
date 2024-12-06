using EmployeeManagement.Shared.Enum;

namespace EmployeeManagement.Shared.Result
{
    public sealed record Error(ErrorType ErrorType, string Discription)
    {
        public static readonly Error None = new(ErrorType.Validation, string.Empty);
        public static readonly Error NullValue = new(ErrorType.Validation, string.Empty);

        public static implicit operator Result(Error error) => Result.Failure(error);
    }
}
