using EmployeeManagement.Database.Infrastructure;
using EmployeeManagement.Shared;
using EmployeeManagement.Shared.Enum;
using EmployeeManagement.Shared.Result;
using MediatR;
using Microsoft.Extensions.Options;
using Utilities.Content;

namespace EmployeeManagement.Features.Employee.UpdateEmployee;

public static partial class UpdateEmployee
{
    internal sealed class Handler : IRequestHandler<Command, Result>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly AppSettings _appSettings;

        public Handler(
            IUnitOfWork unitOfWork,
            IOptions<AppSettings> appSettings)
        {
            _unitOfWork = unitOfWork;
            _appSettings = appSettings.Value;
        }
        public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
        {
            if (request is null)
            {
                return Result.Failure(new(ErrorType.Validation, ContentLoader.ReturnLanguageData("EMP103")));
            }

            if (request.File is not null)
            {
                var extensions = new string[2] { ".jpg", ".png" };

                var extention = Path.GetExtension(request.File.FileName);

                if (!extensions.Contains(extention))
                {
                    return Result.Failure(new(ErrorType.Validation, ContentLoader.ReturnLanguageData("EMP200")));
                }
            }

            var employee = await _unitOfWork.EmployeeRepository.GetFirstOrDefaultAsync(x => x.Id == request.UpdateEmployeeRequest.Row_Id);

            if (employee is null)
            {
                return Result.Failure(new(ErrorType.Validation, ContentLoader.ReturnLanguageData("EMP104")));
            }

            var isMobileNumberExists = await _unitOfWork.EmployeeRepository
                .AnyAsync(x => x.MobileNumber == request.UpdateEmployeeRequest.MobileNumber && x.Id != request.UpdateEmployeeRequest.Row_Id);

            if (isMobileNumberExists)
            {
                return Result.Failure(new(ErrorType.Validation, ContentLoader.ReturnLanguageData("EMP201")));
            }

            var isEmailExists = await _unitOfWork.EmployeeRepository
                .AnyAsync(x => x.EmailAddress == request.UpdateEmployeeRequest.EmailAddress && x.Id != request.UpdateEmployeeRequest.Row_Id);

            if (isEmailExists)
            {
                return Result.Failure(new(ErrorType.Validation, ContentLoader.ReturnLanguageData("EMP202")));
            }

            var isPanNumberExists = await _unitOfWork.EmployeeRepository
                .AnyAsync(x => x.PanNumber == request.UpdateEmployeeRequest.PanNumber && x.Id != request.UpdateEmployeeRequest.Row_Id);

            if (isPanNumberExists)
            {
                return Result.Failure(new(ErrorType.Validation, ContentLoader.ReturnLanguageData("EMP203")));
            }

            var isPassportNumberExists = await _unitOfWork.EmployeeRepository
                .AnyAsync(x => x.PassportNumber == request.UpdateEmployeeRequest.PassportNumber && x.Id != request.UpdateEmployeeRequest.Row_Id);

            if (isPassportNumberExists)
            {
                return Result.Failure(new(ErrorType.Validation, ContentLoader.ReturnLanguageData("EMP204")));
            }

            var oldFileName = employee.ProfileImage;

            employee.FirstName = request.UpdateEmployeeRequest.FirstName;
            employee.LastName = request.UpdateEmployeeRequest.LastName;
            employee.EmailAddress = request.UpdateEmployeeRequest.EmailAddress;
            employee.MobileNumber = request.UpdateEmployeeRequest.MobileNumber;
            employee.PanNumber = request.UpdateEmployeeRequest.PanNumber;
            employee.PassportNumber = request.UpdateEmployeeRequest.PassportNumber;
            employee.CountryId = request.UpdateEmployeeRequest.CountryId;
            employee.StateId = request.UpdateEmployeeRequest.StateId;
            employee.CityId = request.UpdateEmployeeRequest.CityId;
            employee.DateOfBirth = request.UpdateEmployeeRequest.DateOfBirth;
            employee.DateOfJoinee = request.UpdateEmployeeRequest.DateOfJoinee;
            employee.UpdatedDate = DateTime.Now;
            employee.Gender = request.UpdateEmployeeRequest.Gender;
            employee.IsActive = request.UpdateEmployeeRequest.IsActive;
            employee.ProfileImage = request.File != null
                    ? await Utils.SaveFileAsync(request.File)
                    : employee.ProfileImage;

            _unitOfWork.EmployeeRepository.Update(employee);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            Utils.DeleteFile(Path.Combine(_appSettings.ProfilePicturePath, oldFileName));

            return Result.Success();
        }
    }
}
