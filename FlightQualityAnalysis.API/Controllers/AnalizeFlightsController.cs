using FlightQualityAnalysis.FTPService.Model;
using FlightQualityAnalysis.FTPService.Services;
using FlightQualityAnalysis.FTPService.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FlightQualityAnalysis.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnalizeFlightsController : ControllerBase
    {
        private readonly ILogger<AnalizeFlightsController> _logger;
        private readonly IFlightInfoService _flightInfoService;
        private readonly IConfiguration _configuration;
        private readonly IFlightInconsistencyChecker _flightInconsistencyChecker;

        public AnalizeFlightsController(ILogger<AnalizeFlightsController> logger, IFlightInfoService flightInfoService,
            IConfiguration configuration, IFlightInconsistencyChecker flightInconsistencyChecker)
        {
            _logger = logger;
            _flightInfoService = flightInfoService;
            _configuration = configuration;
            _flightInconsistencyChecker = flightInconsistencyChecker;
        }

        /// <summary>
        /// Get flight details from the FTP server and return as JSON.
        /// </summary>
        /// <returns>A list of flight details</returns>

        [HttpGet(Name = "GetFlights")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(IEnumerable<FlightInfo>))]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> Get()
        {
            _logger.LogInformation($"Get flight request.");

            var remoteFilePath = GetRemoteFilePath();
            if (string.IsNullOrEmpty(remoteFilePath))
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "Remote file path is not configured.");
            }

            var flightInfo = await _flightInfoService.ReadCsvWithFluentFtpAndCsvHelper(remoteFilePath);
            if (flightInfo == null || !flightInfo.Any())
            {
                _logger.LogWarning("No flight data found.");
                return NoContent();
            }
            return Ok(flightInfo);
        }

        /// <summary>
        /// Analyze flight inconsistencies such as mismatches between arrival and departure airports.
        /// </summary>
        /// <returns>A list of inconsistencies found</returns>
        [HttpGet("inconsistencies", Name = "AnalyseInconistencies")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(List<string>))]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> GetInconistencies()
        {
            _logger.LogInformation($"Get inconsistensies endpoint called.");

            var remoteFilePath = GetRemoteFilePath();
            if (string.IsNullOrEmpty(remoteFilePath))
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "Remote file path is not configured.");
            }

            var flightInfo = await _flightInfoService.ReadCsvWithFluentFtpAndCsvHelper(remoteFilePath);

            if (flightInfo == null || !flightInfo.Any())
            {
                _logger.LogWarning("No flight data found.");
                return NoContent();
            }

            var inconsistencies = _flightInconsistencyChecker.CheckForInconsistencies(flightInfo);
            if (!inconsistencies.Any())
            {
                return Ok("No inconsistencies found.");
            }

            return Ok(inconsistencies);

        }

        /// <summary>
        /// Retrieves the remote file path from configuration settings.
        /// </summary>
        /// <returns>The remote file path as a string</returns>
        private string? GetRemoteFilePath()
        {
            var remoteFilePath = _configuration["FtpSettings:RemoteFilePath"];
            if (string.IsNullOrEmpty(remoteFilePath))
            {
                _logger.LogError("Remote file path is not configured.");
            }

            return remoteFilePath;
        }
    }
}
