using FlightQualityAnalysis.FTPService.Model;
using FlightQualityAnalysis.FTPService.Services.Interfaces;

namespace FlightQualityAnalysis.FTPService.Services
{
    public class FlightInfoService
    {
        private readonly IFtpClientService _ftpClientService;
        private readonly ICsvFlightInfoParser _flightInfoParser;

        public FlightInfoService(IFtpClientService ftpClientService, ICsvFlightInfoParser flightInfoParser)
        {
            _ftpClientService = ftpClientService;
            _flightInfoParser = flightInfoParser;
        }

        public IEnumerable<FlightInfo> ReadCsvWithFluentFtpAndCsvHelper(string remoteFilePath)
        {
            _ftpClientService.Connect();

            var fileStream = _ftpClientService.DownloadFile(remoteFilePath);

            var flightRecords = _flightInfoParser.ParseCsv(fileStream);

            _ftpClientService.Disconnect();

            return flightRecords;
        }

    }
}
