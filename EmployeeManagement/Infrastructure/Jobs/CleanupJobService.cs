
namespace EmployeeManagement.Infrastructure.Jobs
{
    public class CleanupJobService : IHostedService, IDisposable
    {
        private Timer _timer;
        private readonly ILogger<CleanupJobService> _logger;
        private readonly SemaphoreSlim _semaphoreSlim = new(1, 1);

        public CleanupJobService(ILogger<CleanupJobService> logger)
        {
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(DeleteOpts, null, TimeSpan.Zero, TimeSpan.FromMinutes(2));
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        private void DeleteOpts(object state)
        {
            if (!_semaphoreSlim.Wait(0))
                return;

            try
            {
                _logger.LogInformation($"{DateTime.Now:dd MMM yyyy}: Expired OTPs Deleted.");
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                _semaphoreSlim.Release();
            }
        }

        public void Dispose()
        {
            _timer?.Dispose();
            _semaphoreSlim?.Dispose();
        }
    }
}
