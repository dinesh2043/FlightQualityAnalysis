using FlightQualityAnalysis.FTPService.Services;
using FlightQualityAnalysis.FTPService.Services.Interfaces;
using FluentFTP;

namespace FlightQualityAnalysis.API
{
    public static class ServicesExtension
    {
        public static IServiceCollection AddFlightQualityAnalysisServices(this IServiceCollection services, 
            IConfiguration configuration) 
        {
            // Register the FtpClient in the DI container
            services.AddTransient(serviceProvider =>
            {
                var host = Environment.GetEnvironmentVariable("FTP_HOST");
                var username = Environment.GetEnvironmentVariable("FTP_USERNAME");
                var password = Environment.GetEnvironmentVariable("FTP_PASSWORD");

                if (string.IsNullOrEmpty(host) || string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                {
                    throw new InvalidOperationException("FTP credentials are not configured properly.");
                }

                var ftpClient = new AsyncFtpClient(host, username, password)
                {
                    Config = { EncryptionMode = FtpEncryptionMode.Auto, ValidateAnyCertificate = true }
                };
                return ftpClient;
            });

            services.AddTransient<ICsvFlightInfoParser, CsvFlightInfoParser>();
            services.AddScoped<IFtpClientService, FtpClientService>();
            services.AddTransient<IFlightInfoService, FlightInfoService>();
            services.AddTransient<IFlightInconsistencyChecker, FlightInconsistencyChecker>();
            return services;
        }
    }
}
