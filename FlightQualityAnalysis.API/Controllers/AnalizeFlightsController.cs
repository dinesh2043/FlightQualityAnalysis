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

        public AnalizeFlightsController(ILogger<AnalizeFlightsController> logger, FlightInfoService flightInfoService)
        {
            _logger = logger;
            _flightInfoService = flightInfoService;
        }

        /// <summary>
        /// Get Flights Details
        /// </summary>
        /// It receives a request to download a csv file from ftp server and return it as Json
        /// <returns></returns>

        [HttpGet(Name = "GetFlightsAnalized")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(IEnumerable<FlightInfo>))]
        public IActionResult Get()
        {
            _logger.LogInformation($"Get flight analized called.");
            var flightInfo = _flightInfoService.ReadCsvWithFluentFtpAndCsvHelper("/flights.csv");
            return Ok(flightInfo);
        }
    }
}
