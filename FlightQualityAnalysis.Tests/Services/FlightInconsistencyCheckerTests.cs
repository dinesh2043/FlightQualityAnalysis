
using FakeItEasy;
using FlightQualityAnalysis.FTPService.Model;
using FlightQualityAnalysis.FTPService.Services;
using Microsoft.Extensions.Logging;

namespace FlightQualityAnalysis.Tests.Services
{
    public class FlightInconsistencyCheckerTests
    {
        private readonly FlightInconsistencyChecker _checker;
        private readonly ILogger<FlightInconsistencyChecker> _logger;

        public FlightInconsistencyCheckerTests()
        {
            _logger = A.Fake<ILogger<FlightInconsistencyChecker>>();
            _checker = new FlightInconsistencyChecker(_logger);
        }

        [Fact]
        public void CheckForInconsistencies_WithNoInconsistencies_ShouldReturnEmptyList()
        {
            // Arrange
            var flightInfo = new List<FlightInfo>
            {
                new FlightInfo
                {
                    AircraftRegistrationNumber = "ABC123",
                    FlightNumber = "FL001",
                    DepartureDatetime = DateTime.Parse("2023-09-01 08:00"),
                    ArrivalDatetime = DateTime.Parse("2023-09-01 10:00"),
                    DepartureAirport = "JFK",
                    ArrivalAirport = "LAX"
                },
                new FlightInfo
                {
                    AircraftRegistrationNumber = "ABC123",
                    FlightNumber = "FL001",
                    DepartureDatetime = DateTime.Parse("2023-09-01 12:00"),
                    ArrivalDatetime = DateTime.Parse("2023-09-01 14:00"),
                    DepartureAirport = "LAX",
                    ArrivalAirport = "SFO"
                }
            };

            // Act
            var result = _checker.CheckForInconsistencies(flightInfo);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void CheckForInconsistencies_WithInconsistency_ShouldReturnListWithInconsistencies()
        {
            // Arrange
            var flightInfo = new List<FlightInfo>
            {
                new FlightInfo
                {
                    AircraftRegistrationNumber = "ABC123",
                    FlightNumber = "FL001",
                    DepartureDatetime = DateTime.Parse("2023-09-01 08:00"),
                    ArrivalDatetime = DateTime.Parse("2023-09-01 10:00"),
                    DepartureAirport = "JFK",
                    ArrivalAirport = "LAX"
                },
                new FlightInfo
                {
                    AircraftRegistrationNumber = "ABC123",
                    FlightNumber = "FL001",
                    DepartureDatetime = DateTime.Parse("2023-09-01 12:00"),
                    ArrivalDatetime = DateTime.Parse("2023-09-01 14:00"),
                    DepartureAirport = "SFO", // Inconsistent departure airport
                    ArrivalAirport = "SEA"
                }
            };

            // Act
            var result = _checker.CheckForInconsistencies(flightInfo);

            // Assert
            Assert.Single(result);
            Assert.Contains("Inconsistency for aircraft ABC123", result[0]);
        }

        [Fact]
        public void CheckForInconsistencies_WithMultipleAircraft_ShouldReturnCorrectInconsistencies()
        {
            // Arrange
            var flightInfo = new List<FlightInfo>
            {
                new FlightInfo
                {
                    AircraftRegistrationNumber = "ABC123",
                    FlightNumber = "FL001",
                    DepartureDatetime = DateTime.Parse("2023-09-01 08:00"),
                    ArrivalDatetime = DateTime.Parse("2023-09-01 10:00"),
                    DepartureAirport = "JFK",
                    ArrivalAirport = "LAX"
                },
                new FlightInfo
                {
                    AircraftRegistrationNumber = "ABC123",
                    FlightNumber = "FL001",
                    DepartureDatetime = DateTime.Parse("2023-09-01 12:00"),
                    ArrivalDatetime = DateTime.Parse("2023-09-01 14:00"),
                    DepartureAirport = "LAX",
                    ArrivalAirport = "SFO"
                },
                new FlightInfo
                {
                    AircraftRegistrationNumber = "XYZ987",
                    FlightNumber = "FL001",
                    DepartureDatetime = DateTime.Parse("2023-09-01 08:00"),
                    ArrivalDatetime = DateTime.Parse("2023-09-01 10:00"),
                    DepartureAirport = "SFO",
                    ArrivalAirport = "ORD"
                },
                new FlightInfo
                {
                    AircraftRegistrationNumber = "XYZ987",
                    FlightNumber = "FL001",
                    DepartureDatetime = DateTime.Parse("2023-09-01 12:00"),
                    ArrivalDatetime = DateTime.Parse("2023-09-01 14:00"),
                    DepartureAirport = "SEA", // Inconsistent departure airport
                    ArrivalAirport = "DFW"
                }
            };

            // Act
            var result = _checker.CheckForInconsistencies(flightInfo);

            // Assert
            Assert.Single(result); // Only one inconsistency for XYZ987
            Assert.Contains("Inconsistency for aircraft XYZ987", result[0]);
        }

        [Fact]
        public void CheckForInconsistencies_WithEmptyFlightList_ShouldReturnEmptyList()
        {
            // Arrange
            var flightInfo = new List<FlightInfo>();

            // Act
            var result = _checker.CheckForInconsistencies(flightInfo);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void CheckForInconsistencies_WhenFlightInfoIsNull_ShouldThrowArgumentNullException()
        {
            //Arrange
            List<FlightInfo>? flightInfo = null;
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _checker.CheckForInconsistencies(flightInfo!));
        }
    }
}
