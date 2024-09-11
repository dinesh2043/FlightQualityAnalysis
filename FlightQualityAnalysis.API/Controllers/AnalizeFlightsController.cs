using FlightQualityAnalysis.FTPService.Model;
using FlightQualityAnalysis.FTPService.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FlightQualityAnalysis.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnalizeFlightsController : ControllerBase
    {
        private readonly ILogger<AnalizeFlightsController> _logger;
        private readonly FlightInfoService _flightInfoService;
        private readonly IConfiguration _configuration;

        public AnalizeFlightsController(ILogger<AnalizeFlightsController> logger, FlightInfoService flightInfoService,
            IConfiguration configuration)
        {
            _logger = logger;
            _flightInfoService = flightInfoService;
            _configuration = configuration;
        }

        /// <summary>
        /// Get Flights Details
        /// </summary>
        /// It receives a request to download a csv file from ftp server and return it as Json
        /// <returns></returns>

        [HttpGet(Name = "GetFlights")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(IEnumerable<FlightInfo>))]
        public IActionResult Get()
        {
            _logger.LogInformation($"Get flight request.");

            // Retrieve the remote file path from the configuration
            var remoteFilePath = _configuration["FtpSettings:RemoteFilePath"];

            if (string.IsNullOrEmpty(remoteFilePath))
            {
                _logger.LogError("Remote file path is not configured.");
                return StatusCode((int)HttpStatusCode.InternalServerError, "Remote file path is not configured.");
            }

            var flightInfo = _flightInfoService.ReadCsvWithFluentFtpAndCsvHelper(remoteFilePath);
            return Ok(flightInfo);
        }
    }
}
