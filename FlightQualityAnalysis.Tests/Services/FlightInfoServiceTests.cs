using FlightQualityAnalysis.FTPService.Services.Interfaces;
using FlightQualityAnalysis.FTPService.Services;
using Microsoft.Extensions.Logging;
using FakeItEasy;
using FlightQualityAnalysis.FTPService.Model;

namespace FlightQualityAnalysis.Tests.Services
{
    public class FlightInfoServiceTests
    {
        private readonly FlightInfoService _flightInfoService;
        private readonly IFtpClientService _fakeFtpClientService;
        private readonly ICsvFlightInfoParser _fakeFlightInfoParser;
        private readonly ILogger<FlightInfoService> _fakeLogger;

        public FlightInfoServiceTests()
        {
            _fakeFtpClientService = A.Fake<IFtpClientService>();
            _fakeFlightInfoParser = A.Fake<ICsvFlightInfoParser>();
            _fakeLogger = A.Fake<ILogger<FlightInfoService>>();

            _flightInfoService = new FlightInfoService(
                _fakeFtpClientService,
                _fakeFlightInfoParser,
                _fakeLogger
            );
        }

        [Fact]
        public async Task ReadCsvWithFluentFtpAndCsvHelper_ShouldConnectAndDisconnectFromFtp()
        {
            // Arrange
            var remoteFilePath = "remote/path/to/file.csv";
            var fakeStream = new MemoryStream();
            var fakeFlightRecords = new List<FlightInfo>
            {
                new FlightInfo
                {
                    AircraftRegistrationNumber = "ABC001", // Required property
                    FlightNumber = "FN001",
                    DepartureDatetime = DateTime.UtcNow,
                    ArrivalAirport = "LAX",
                    DepartureAirport = "JFK"
                }
            };

            A.CallTo(() => _fakeFtpClientService.DownloadFile(remoteFilePath)).Returns(fakeStream);
            A.CallTo(() => _fakeFlightInfoParser.ParseCsv(fakeStream)).Returns(Task.FromResult((IEnumerable<FlightInfo>)fakeFlightRecords));

            // Act
            var result = await _flightInfoService.ReadCsvWithFluentFtpAndCsvHelper(remoteFilePath);

            // Assert
            A.CallTo(() => _fakeFtpClientService.Connect()).MustHaveHappenedOnceExactly();
            A.CallTo(() => _fakeFtpClientService.DownloadFile(remoteFilePath)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _fakeFlightInfoParser.ParseCsv(fakeStream)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _fakeFtpClientService.Disconnect()).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task ReadCsvWithFluentFtpAndCsvHelper_ShouldReturnFlightInfoRecords()
        {
            // Arrange
            var remoteFilePath = "remote/path/to/file.csv";
            var fakeStream = new MemoryStream();
            var fakeFlightRecords = new List<FlightInfo>
            {
                new FlightInfo
                {
                    AircraftRegistrationNumber = "ABC002", // Required property
                    FlightNumber = "FN002",
                    DepartureDatetime = DateTime.UtcNow,
                    ArrivalAirport = "KTM",
                    DepartureAirport = "HEL"
                }
            };

            A.CallTo(() => _fakeFtpClientService.DownloadFile(remoteFilePath)).Returns(fakeStream);
            A.CallTo(() => _fakeFlightInfoParser.ParseCsv(fakeStream)).Returns(Task.FromResult((IEnumerable<FlightInfo>)fakeFlightRecords));

            // Act
            var result = await _flightInfoService.ReadCsvWithFluentFtpAndCsvHelper(remoteFilePath);

            // Assert
            Assert.Equal(fakeFlightRecords, result);
        }

        [Fact]
        public async Task ReadCsvWithFluentFtpAndCsvHelper_ShouldCallDownloadFile()
        {
            // Arrange
            var remoteFilePath = "path/to/file.csv";
            var fakeStream = new MemoryStream();
            var fakeFlightRecords = new List<FlightInfo> { new FlightInfo { AircraftRegistrationNumber = "ABC123" } };

            A.CallTo(() => _fakeFtpClientService.DownloadFile(remoteFilePath))
                .Returns(fakeStream);

            A.CallTo(() => _fakeFlightInfoParser.ParseCsv(fakeStream))
                .Returns(fakeFlightRecords);

            // Act
            var result = await _flightInfoService.ReadCsvWithFluentFtpAndCsvHelper(remoteFilePath);

            // Assert
            A.CallTo(() => _fakeFtpClientService.DownloadFile(remoteFilePath))
                .MustHaveHappenedOnceExactly();

            A.CallTo(() => _fakeFlightInfoParser.ParseCsv(fakeStream))
                .MustHaveHappenedOnceExactly();

            Assert.Equal(fakeFlightRecords, result);
        }
    }
}
