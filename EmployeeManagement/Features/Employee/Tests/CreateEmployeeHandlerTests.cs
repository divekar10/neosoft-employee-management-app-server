using EmployeeManagement.Contracts.Employee;
using EmployeeManagement.Database.Infrastructure;
using EmployeeManagement.Features.EmployeeFeatures;
using EmployeeManagement.Shared.Enum;
using MediatR;
using Moq;
using Xunit;
using System.Linq;
using System.Linq.Expressions;

namespace EmployeeManagement.Features.Employee.Tests
{
    public class CreateEmployeeHandlerTests
    {
        private readonly Mock<IUnitOfWork> _mocUnitOfWork;
        private readonly Mock<IPublisher> _mocPublisher;
        private readonly CreateEmployee.Handler _handler;
        public CreateEmployeeHandlerTests()
        {
            _mocUnitOfWork = new Mock<IUnitOfWork>();
            _mocPublisher = new Mock<IPublisher>();
            _handler = new CreateEmployee.Handler(_mocUnitOfWork.Object, _mocPublisher.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnValidationError_WhenRequestIsNull()
        {
            // Arrange
            CreateEmployee.Command request = null;

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorType.Validation, result.Error.ErrorType);
        }

        [Fact]
        public async Task Handle_ShouldReturnValidationError_WhenProfilePictureExtenstionIsNotValid()
        {
            var file = new Mock<IFormFile>();

            var request = new CreateEmployee.Command
            {
                AddEmployeeRequest = new AddEmployeeRequest
                {
                    FirstName = "John",
                    LastName = "Doe",
                    EmailAddress = "john.doe@example.com",
                    MobileNumber = "1234567890",
                    PanNumber = "ABCDE1234F",
                    PassportNumber = "A1234567",
                    CityId = 1,
                    CountryId = 1,
                    StateId = 1,
                    Gender = 1,
                    DateOfBirth = DateTime.Parse("1990-01-01"),
                    DateOfJoinee = DateTime.Now,
                    IsActive = true
                },
                ProfileImage = file.Object
            };

            file.Setup(x => x.FileName).Returns("test.bin").Verifiable();

            var result = await _handler.Handle(request, CancellationToken.None);

            Assert.NotNull(result);
            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorType.Validation, result.Error.ErrorType);
        }

        [Fact]
        public async Task Handle_ShouldReturnValidationError_WhenEmailAlreadyExists()
        {
            var request = new CreateEmployee.Command
            {
                AddEmployeeRequest = new AddEmployeeRequest
                {
                    FirstName = "John",
                    LastName = "Doe",
                    EmailAddress = "akash@gmail.com",
                    MobileNumber = "1234567890",
                    PanNumber = "ABCDE1234F",
                    PassportNumber = "A1234567",
                    CityId = 1,
                    CountryId = 1,
                    StateId = 1,
                    Gender = 1,
                    DateOfBirth = DateTime.Parse("1990-01-01"),
                    DateOfJoinee = DateTime.Now,
                    IsActive = true
                },
                ProfileImage = null
            };

            _mocUnitOfWork.Setup(u => u.EmployeeRepository);

            var result = await _handler.Handle(request, CancellationToken.None);

            Assert.NotNull(result);
            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorType.Validation, result.Error.ErrorType);
        }
    }
}
