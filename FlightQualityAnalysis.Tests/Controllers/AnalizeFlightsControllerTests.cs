using Microsoft.Extensions.Configuration;
using FlightQualityAnalysis.FTPService.Services.Interfaces;
using Microsoft.Extensions.Logging;
using FlightQualityAnalysis.API.Controllers;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using FlightQualityAnalysis.FTPService.Model;

namespace FlightQualityAnalysis.Tests.Controllers
{
    public class AnalizeFlightsControllerTests
    {
        private readonly AnalizeFlightsController _controller;
        private readonly ILogger<AnalizeFlightsController> _logger;
        private readonly IFlightInfoService _flightInfoService;
        private readonly IConfiguration _configuration;
        private readonly IFlightInconsistencyChecker _flightInconsistencyChecker;

        public AnalizeFlightsControllerTests()
        {
            _logger = A.Fake<ILogger<AnalizeFlightsController>>();
            _flightInfoService = A.Fake<IFlightInfoService>();
            _configuration = A.Fake<IConfiguration>();
            _flightInconsistencyChecker = A.Fake<IFlightInconsistencyChecker>();

            _controller = new AnalizeFlightsController(
                _logger,
                _flightInfoService,
                _configuration,
                _flightInconsistencyChecker
            );
        }

        [Fact]
        public async Task Get_WhenRemoteFilePathIsNull_ShouldReturnInternalServerError()
        {
            // Arrange
            String? remoteFilePath = null;
            A.CallTo(() => _configuration["FtpSettings:RemoteFilePath"]).Returns(remoteFilePath);

            // Act
            var result = await _controller.Get();

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCodeResult.StatusCode);
            Assert.Equal("Remote file path is not configured.", statusCodeResult.Value);
        }

        [Fact]
        public async Task Get_WhenFlightInfoIsEmpty_ShouldReturnNoContent()
        {
            // Arrange
            A.CallTo(() => _configuration["FtpSettings:RemoteFilePath"]).Returns("validPath");
            A.CallTo(() => _flightInfoService.ReadCsvWithFluentFtpAndCsvHelper("validPath")).Returns(new List<FlightInfo>());

            // Act
            var result = await _controller.Get();

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Get_WhenFlightInfoIsReturned_ShouldReturnOk()
        {
            // Arrange
            var flightInfo = new List<FlightInfo> { new FlightInfo { AircraftRegistrationNumber = "ABC123" } };
            A.CallTo(() => _configuration["FtpSettings:RemoteFilePath"]).Returns("validPath");
            A.CallTo(() => _flightInfoService.ReadCsvWithFluentFtpAndCsvHelper("validPath")).Returns(flightInfo);

            // Act
            var result = await _controller.Get();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(flightInfo, okResult.Value);
        }

        [Fact]
        public async Task GetInconistencies_WhenRemoteFilePathIsNull_ShouldReturnInternalServerError()
        {
            // Arrange
            String? remoteFilePath = null;
            A.CallTo(() => _configuration["FtpSettings:RemoteFilePath"]).Returns(remoteFilePath);

            // Act
            var result = await _controller.GetInconistencies();

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCodeResult.StatusCode);
            Assert.Equal("Remote file path is not configured.", statusCodeResult.Value);
        }

        [Fact]
        public async Task GetInconistencies_WhenFlightInfoIsEmpty_ShouldReturnNoContent()
        {
            // Arrange
            A.CallTo(() => _configuration["FtpSettings:RemoteFilePath"]).Returns("validPath");
            A.CallTo(() => _flightInfoService.ReadCsvWithFluentFtpAndCsvHelper("validPath")).Returns(new List<FlightInfo>());

            // Act
            var result = await _controller.GetInconistencies();

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task GetInconistencies_WhenInconsistenciesAreFound_ShouldReturnOkWithInconsistencies()
        {
            // Arrange
            var flightInfo = new List<FlightInfo> { new FlightInfo { AircraftRegistrationNumber = "ABC123" } };
            var inconsistencies = new List<string> { "Inconsistency found" };

            A.CallTo(() => _configuration["FtpSettings:RemoteFilePath"]).Returns("validPath");
            A.CallTo(() => _flightInfoService.ReadCsvWithFluentFtpAndCsvHelper("validPath")).Returns(flightInfo);
            A.CallTo(() => _flightInconsistencyChecker.CheckForInconsistencies(flightInfo)).Returns(inconsistencies);

            // Act
            var result = await _controller.GetInconistencies();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(inconsistencies, okResult.Value);
        }

        [Fact]
        public async Task GetInconistencies_WhenNoInconsistenciesFound_ShouldReturnOkWithNoInconsistenciesMessage()
        {
            // Arrange
            var flightInfo = new List<FlightInfo> { new FlightInfo { AircraftRegistrationNumber = "ABC123" } };

            A.CallTo(() => _configuration["FtpSettings:RemoteFilePath"]).Returns("validPath");
            A.CallTo(() => _flightInfoService.ReadCsvWithFluentFtpAndCsvHelper("validPath")).Returns(flightInfo);
            A.CallTo(() => _flightInconsistencyChecker.CheckForInconsistencies(flightInfo)).Returns(new List<string>());

            // Act
            var result = await _controller.GetInconistencies();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("No inconsistencies found.", okResult.Value);
        }

    }
}
