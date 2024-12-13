using MediatR;
using Polly;

namespace EmployeeManagement.Features.Employee.Event
{
    public class EmployeeCreated
    {
        public class EmployeeCreatedNotification : INotification
        {
            public int Row_Id { get; set; }
            public string EmailAddress { get; set; }
            public string FirstName { get; set; }
            public EmployeeCreatedNotification(int row_Id, string emailAddress, string firstName)
            {
                Row_Id = row_Id;
                EmailAddress = emailAddress;
                FirstName = firstName;
            }
        }

        internal sealed class EmployeeCreatedNotificationHandler : INotificationHandler<EmployeeCreatedNotification>
        {
            private readonly ILogger<EmployeeCreatedNotificationHandler> _logger;

            public EmployeeCreatedNotificationHandler(ILogger<EmployeeCreatedNotificationHandler> logger)
            {
                _logger = logger;
            }
            public async Task Handle(EmployeeCreatedNotification notification, CancellationToken cancellationToken)
            {
                var retryPolicy = Policy
                    .Handle<Exception>()
                    .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

                await retryPolicy.ExecuteAsync(async () =>
                {
                    _logger.LogInformation("Sent welcome email.");
                    await Task.FromResult(0);
                });
            }
        }
    }
}
