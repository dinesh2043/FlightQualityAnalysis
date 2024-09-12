using FlightQualityAnalysis.FTPService.Model;
using FlightQualityAnalysis.FTPService.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace FlightQualityAnalysis.FTPService.Services
{
    public class FlightInfoService
    {
        private readonly IFtpClientService _ftpClientService;
        private readonly ICsvFlightInfoParser _flightInfoParser;
        private readonly ILogger<FlightInfoService> _logger;

        public FlightInfoService(IFtpClientService ftpClientService, ICsvFlightInfoParser flightInfoParser,
            ILogger<FlightInfoService> logger)
        {
            _ftpClientService = ftpClientService;
            _flightInfoParser = flightInfoParser;
            _logger = logger;
        }

        public async Task<IEnumerable<FlightInfo>> ReadCsvWithFluentFtpAndCsvHelper(string remoteFilePath)
        {
            await _ftpClientService.Connect();

            var fileStream = await _ftpClientService.DownloadFile(remoteFilePath);

            var flightRecords = await _flightInfoParser.ParseCsv(fileStream);

            _logger.LogInformation("Csv file parser called.");

            await _ftpClientService.Disconnect();

            return flightRecords;
        }

    }
}
