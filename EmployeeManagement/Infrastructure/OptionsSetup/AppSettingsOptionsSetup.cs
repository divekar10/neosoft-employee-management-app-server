using EmployeeManagement.Shared;
using EmployeeManagement.Shared.Constants;
using Microsoft.Extensions.Options;

namespace EmployeeManagement.Infrastructure.OptionsSetup
{
    public class AppSettingsOptionsSetup(IConfiguration configuration) : IConfigureOptions<AppSettings>
    {
        private readonly IConfiguration _configuration = configuration;
        public void Configure(AppSettings options)
        {
            _configuration.GetSection(Constants.APP_SETTINGS_KEY)
                .Bind(options);
        }
    }
}
