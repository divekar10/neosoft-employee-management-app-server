using AutoMapper;
using EmployeeManagement.Contracts.Employee;
using EmployeeManagement.Contracts.EmployeeFeature;
using EmployeeManagement.Features.Employee.DeleteEmployee;
using EmployeeManagement.Features.Employee.GetEmployees;
using EmployeeManagement.Features.Employee.UpdateEmployee;
using EmployeeManagement.Features.EmployeeFeatures;

namespace EmployeeManagement.Infrastructure.AutoMapper
{
    public class EmployeeProfile : Profile
    {
        public EmployeeProfile()
        {
            CreateMap<AddEmployeeRequest, CreateEmployee.Command>().ReverseMap();
            CreateMap<UpdateEmployeeRequest, UpdateEmployee.Command>().ReverseMap();
            CreateMap<DeleteEmployeeRequest, DeleteEmployee.Command>().ReverseMap();
            CreateMap<GetEmployeesRequest, GetEmployees.Query>().ReverseMap();
        }
    }
}
