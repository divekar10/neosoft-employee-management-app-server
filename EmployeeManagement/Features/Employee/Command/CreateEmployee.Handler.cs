using EmployeeManagement.Database.Infrastructure;
using EmployeeManagement.Entities;
using EmployeeManagement.Features.Employee.Event;
using EmployeeManagement.Shared;
using EmployeeManagement.Shared.Enum;
using EmployeeManagement.Shared.Result;
using MediatR;
using Utilities.Content;
using static EmployeeManagement.Features.Employee.Event.EmployeeCreated;

namespace EmployeeManagement.Features.EmployeeFeatures;

public static partial class CreateEmployee
{
    internal sealed class Handler
        : IRequestHandler<Command, Result>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPublisher _publisher;

        public Handler(
            IUnitOfWork unitOfWork, 
            IPublisher publisher)
        {
            _unitOfWork = unitOfWork;
            _publisher = publisher;
        }
        public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
        {
            if (request is null)
            {
                return Result.Failure(new(ErrorType.Validation, ContentLoader.ReturnLanguageData("EMP103")));
            }

            if (request.ProfileImage is not null)
            {
                var extensions = new string[2] { ".jpg", ".png" };

                var extention = Path.GetExtension(request.ProfileImage.FileName);

                if (!extensions.Contains(extention))
                {
                    return Result.Failure(new(ErrorType.Validation, ContentLoader.ReturnLanguageData("EMP200")));
                }
            }


            var isMobileNumberExists = await _unitOfWork.EmployeeRepository
                .AnyAsync(x => x.MobileNumber == request.AddEmployeeRequest.MobileNumber);

            if (isMobileNumberExists)
            {
                return Result.Failure(new(ErrorType.Validation, ContentLoader.ReturnLanguageData("EMP201")));
            }

            var isEmailExists = await _unitOfWork.EmployeeRepository
                .AnyAsync(x => x.EmailAddress == request.AddEmployeeRequest.EmailAddress);

            if (isEmailExists)
            {
                return Result.Failure(new(ErrorType.Validation, ContentLoader.ReturnLanguageData("EMP202")));
            }

            var isPanNumberExists = await _unitOfWork.EmployeeRepository
                .AnyAsync(x => x.PanNumber == request.AddEmployeeRequest.PanNumber);

            if (isPanNumberExists)
            {
                return Result.Failure(new(ErrorType.Validation, ContentLoader.ReturnLanguageData("EMP203")));
            }

            var isPassportNumberExists = await _unitOfWork.EmployeeRepository
                .AnyAsync(x => x.PassportNumber == request.AddEmployeeRequest.PassportNumber);

            if (isPassportNumberExists)
            {
                return Result.Failure(new(ErrorType.Validation, ContentLoader.ReturnLanguageData("EMP204")));
            }

            bool isExists = true;
            string employeeNumber = string.Empty;
            while (isExists)
            {
                employeeNumber = string.Format("{0}{1}", "00", Utils.GetRandomNumber());

                var isEmployeeCodeExists = await _unitOfWork.EmployeeRepository.AnyAsync(x => x.EmployeeCode == employeeNumber);

                if (!isEmployeeCodeExists)
                {
                    isExists = false;
                }
            }

            var employee = new Entities.Employee()
            {
                FirstName = request.AddEmployeeRequest.FirstName,
                LastName = request.AddEmployeeRequest.LastName,
                EmailAddress = request.AddEmployeeRequest.EmailAddress,
                EmployeeCode = employeeNumber,
                MobileNumber = request.AddEmployeeRequest.MobileNumber,
                PanNumber = request.AddEmployeeRequest.PanNumber,
                CityId = request.AddEmployeeRequest.CityId,
                CountryId = request.AddEmployeeRequest.CountryId,
                StateId = request.AddEmployeeRequest.StateId,
                PassportNumber = request.AddEmployeeRequest.PassportNumber,
                Gender = request.AddEmployeeRequest.Gender,
                DateOfBirth = request.AddEmployeeRequest.DateOfBirth,
                DateOfJoinee = request.AddEmployeeRequest.DateOfJoinee,
                IsActive = request.AddEmployeeRequest.IsActive,
                CreatedDate = DateTime.Now,
                ProfileImage = request.ProfileImage != null
                    ? await Utils.SaveFileAsync(request.ProfileImage)
                    : ""
            };

            _unitOfWork.EmployeeRepository.Add(employee);
            await _unitOfWork.SaveChangesAsync();

            var employeeCreatedNotification = new EmployeeCreatedNotification(employee.Row_Id,
                                                                              employee.EmailAddress,
                                                                              employee.FirstName);

            await _publisher.Publish(employeeCreatedNotification, cancellationToken);

            return Result.Success();
        }
    }
}
