using EmployeeManagement.Contracts.EmployeeFeature;
using EmployeeManagement.Database.Infrastructure;
using EmployeeManagement.Database.Infrastructure.Extensions;
using EmployeeManagement.DTOs.EmployeeDto;
using EmployeeManagement.Entities;
using EmployeeManagement.Features.Employee.GetEmployees;
using EmployeeManagement.Shared;
using EmployeeManagement.Shared.Enum;
using EmployeeManagement.Shared.Result;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace EmployeeManagement.Database.Repositories;

public interface IEmployeeRepository : IRepository<Employee>
{
    Task<Result<GetEmployeeDto>> GetEmployee(int id);
    Task<Result<PaginatedList<GetEmployeeDto>>> GetEmployees(GetEmployees.Query request);
}

public class EmployeeRepository : Repository<Employee>, IEmployeeRepository
{
    public EmployeeRepository(EmployeeDbContext context) : base(context)
    {
    }

    public async Task<Result<GetEmployeeDto>> GetEmployee(int id)
    {
        var employee = await (from e in _context.Employee
                              join c in _context.Country on e.CountryId equals c.Row_Id
                              join s in _context.State on c.Row_Id equals s.CountryId
                              join ci in _context.City on s.Row_Id equals ci.StateId
                              where e.Row_Id == id
                              select new GetEmployeeDto
                              {
                                  FirstName = e.FirstName,
                                  LastName = e.LastName,
                                  EmailAddress = e.EmailAddress,
                                  EmployeeCode = e.EmployeeCode,
                                  MobileNumber = e.MobileNumber,
                                  PanNumber = e.PanNumber,
                                  Gender = e.Gender,
                                  PassportNumber = e.PassportNumber,
                                  Row_Id = e.Row_Id,
                                  IsActive = e.IsActive,
                                  ProfileImage = e.ProfileImage,
                                  CountryName = c.CountryName,
                                  StateName = s.StateName,
                                  CityName = ci.CityName,
                                  CountryId = e.CountryId,
                                  StateId = e.StateId,
                                  CityId = e.CityId,
                                  DateOfBirth = e.DateOfBirth,
                                  DateOfJoinee = e.DateOfJoinee
                              }).FirstOrDefaultAsync();


        if (employee is null)
        {
            return Result.Failure<GetEmployeeDto>(new(ErrorType.NotFound, "record not found."));
        }
        else
        {
            employee.ProfileImage = await Utils.FileToBase64(Path.Combine(Directory.GetCurrentDirectory(), employee.ProfileImage!));

            return Result.Success(employee);
        }
    }

    public async Task<Result<PaginatedList<GetEmployeeDto>>> GetEmployees(GetEmployees.Query request)
    {
        var query = (from e in _context.Employee
                     join c in _context.Country on e.CountryId equals c.Row_Id
                     join s in _context.State on e.StateId equals s.Row_Id
                     join ci in _context.City on e.CityId equals ci.Row_Id
                     where e.IsDeleted == false
                     select new GetEmployeeDto
                     {
                         FirstName = e.FirstName,
                         LastName = e.LastName,
                         EmailAddress = e.EmailAddress,
                         EmployeeCode = e.EmployeeCode,
                         MobileNumber = e.MobileNumber,
                         PanNumber = e.PanNumber,
                         Gender = e.Gender,
                         PassportNumber = e.PassportNumber,
                         Row_Id = e.Row_Id,
                         IsActive = e.IsActive,
                         ProfileImage = e.ProfileImage,
                         CountryName = c.CountryName,
                         StateName = s.StateName,
                         CityName = ci.CityName
                     }).AsQueryable();

        if (!string.IsNullOrEmpty(request.SearchText))
        {
            query = query.Where(x => x.FirstName.Contains(request.SearchText));
        }

        if (!string.IsNullOrEmpty(request.SortBy) && !string.IsNullOrEmpty(request.SortOrder))
        {
            query = query.ApplySorting(request.SortBy, request.SortOrder);
        }

        var result = await PaginatedList<GetEmployeeDto>.CreateAsync(query, request.PageIndex, request.PageSize);

        var currentDirectory = Directory.GetCurrentDirectory();
        result.Items.ForEach(async x =>
        {
            x.ProfileImage = await Utils.FileToBase64(Path.Combine(currentDirectory, x.ProfileImage!));
        });

        //query = query.Skip(request.From)
        //             .Take(request.To - request.From);

        //var employee = await query.Select(e => new GetEmployeeDto
        //{
        //    FirstName = e.FirstName,
        //    LastName = e.LastName,
        //    EmailAddress = e.EmailAddress,
        //    EmployeeCode = e.EmployeeCode,
        //    MobileNumber = e.MobileNumber,
        //    PanNumber = e.PanNumber,
        //    Gender = e.Gender,
        //    PassportNumber = e.PassportNumber,
        //    Row_Id = e.Row_Id,
        //    IsActive = e.IsActive,
        //    ProfileImage = e.ProfileImage,
        //    CountryName = e.CountryName,
        //    StateName = e.StateName,
        //    CityName = e.CityName
        //}).ToListAsync();

        //if (employee is null)
        //{
        //    return Result.Failure<List<GetEmployeeDto>>(new(ErrorType.NotFound, "record not found."));
        //}
        //else
        //{
        return Result.Success(result);
        //}
    }


    //public async Task<Result<List<GetEmployeeDto>>> GetEmployees(GetEmployeesRequest request)
    //{
    //    var query = (from e in _context.Employee
    //                 join c in _context.Country on e.CountryId equals c.Row_Id
    //                 join s in _context.State on e.StateId equals s.Row_Id
    //                 join ci in _context.City on e.CityId equals ci.Row_Id
    //                 where e.IsActive == true && e.IsDeleted == false
    //                 select new GetEmployeeDto
    //                 {
    //                     FirstName = e.FirstName,
    //                     LastName = e.LastName,
    //                     EmailAddress = e.EmailAddress,
    //                     EmployeeCode = e.EmployeeCode,
    //                     MobileNumber = e.MobileNumber,
    //                     PanNumber = e.PanNumber,
    //                     Gender = e.Gender,
    //                     PassportNumber = e.PassportNumber,
    //                     Row_Id = e.Row_Id,
    //                     IsActive = e.IsActive,
    //                     ProfileImage = e.ProfileImage,
    //                     CountryName = c.CountryName,
    //                     StateName = s.StateName,
    //                     CityName = ci.CityName
    //                 }).AsQueryable();

    //    if (!string.IsNullOrEmpty(request.SearchText))
    //    {
    //        query = query.Where(x => x.FirstName == request.SearchText || x.LastName == request.SearchText);
    //    }

    //    if (request.SortOrder == "asc")
    //    {
    //        query = query.OrderBy(x => x.Row_Id);
    //    }
    //    else
    //    {
    //        query = query.OrderByDescending(x => x.Row_Id);
    //    }

    //    query = query.Skip(request.From)
    //                 .Take(request.To - request.From);

    //    var employee = await query.Select(e => new GetEmployeeDto
    //    {
    //        FirstName = e.FirstName,
    //        LastName = e.LastName,
    //        EmailAddress = e.EmailAddress,
    //        EmployeeCode = e.EmployeeCode,
    //        MobileNumber = e.MobileNumber,
    //        PanNumber = e.PanNumber,
    //        Gender = e.Gender,
    //        PassportNumber = e.PassportNumber,
    //        Row_Id = e.Row_Id,
    //        IsActive = e.IsActive,
    //        ProfileImage = e.ProfileImage,
    //        CountryName = e.CountryName,
    //        StateName = e.StateName,
    //        CityName = e.CityName
    //    }).ToListAsync();

    //    if (employee is null)
    //    {
    //        return Result.Failure<List<GetEmployeeDto>>(new(ErrorType.NotFound, "record not found."));
    //    }
    //    else
    //    {
    //        return Result.Success(employee);
    //    }
    //}
}
