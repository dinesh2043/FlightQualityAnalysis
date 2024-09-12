
using FakeItEasy;
using FlightQualityAnalysis.FTPService.Services;
using Microsoft.Extensions.Logging;

namespace FlightQualityAnalysis.Tests.Services
{
    public class CsvFlightInfoParserTests
    {
        private readonly CsvFlightInfoParser _parser;
        private readonly ILogger<CsvFlightInfoParser> _fakeLogger;

        public CsvFlightInfoParserTests()
        {
            _fakeLogger = A.Fake<ILogger<CsvFlightInfoParser>>();
            _parser = new CsvFlightInfoParser(_fakeLogger);
        }

        [Fact]
        public async Task ParseCsv_WithValidStream_ShouldReturnRecords()
        {
            // Arrange
            var csvData = "id,aircraft_registration_number,aircraft_type,flight_number,departure_airport,departure_datetime,arrival_airport,arrival_datetime\n"+
                        "1,ZX-IKD,350,M645,HEL,2024-01-02 21:46:27,DXB,2024-01-03 02:31:27\n";
            var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(csvData));

            // Act
            var result = await _parser.ParseCsv(stream);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Any());
            var flightInfo = result.First();
            Assert.Equal(1, flightInfo.Id);
            Assert.Equal("ZX-IKD", flightInfo.AircraftRegistrationNumber);
            Assert.Equal("350", flightInfo.AircraftType);
            Assert.Equal("M645", flightInfo.FlightNumber);
            Assert.Equal("HEL", flightInfo.DepartureAirport);
            Assert.Equal("DXB", flightInfo.ArrivalAirport);
        }

        [Fact]
        public async Task ParseCsv_WithEmptyStream_ShouldReturnEmptyList()
        {
            // Arrange
            var emptyStream = new MemoryStream();

            // Act
            var result = await _parser.ParseCsv(emptyStream);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public void ParseCsv_WithNullStream_ShouldThrowArgumentNullException()
        {
            // Arrange
            Stream? nullStream = null;

            // Act & Assert
            Assert.ThrowsAsync<ArgumentNullException>(async () => await _parser.ParseCsv(nullStream!));
        }
    }
}
