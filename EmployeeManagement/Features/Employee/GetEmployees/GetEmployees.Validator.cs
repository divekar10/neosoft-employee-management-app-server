using EmployeeManagement.Contracts.EmployeeFeature;
using FluentValidation;

namespace EmployeeManagement.Features.Employee.GetEmployees;

public static partial class GetEmployees
{
    public class Validator : AbstractValidator<GetEmployeesRequest>
    {
        public Validator()
        {

        }
    }
}
