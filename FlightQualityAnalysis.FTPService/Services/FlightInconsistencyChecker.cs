
using FlightQualityAnalysis.FTPService.Model;
using FlightQualityAnalysis.FTPService.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace FlightQualityAnalysis.FTPService.Services
{
    public class FlightInconsistencyChecker : IFlightInconsistencyChecker
    {
        private readonly ILogger<FlightInconsistencyChecker> _logger;

        public FlightInconsistencyChecker(ILogger<FlightInconsistencyChecker> logger)
        {
            _logger = logger;
        }

        public List<string> CheckForInconsistencies(IEnumerable<FlightInfo> flightInfo)
        {
            var groupedFlights = flightInfo
                .GroupBy(f => f.AircraftRegistrationNumber)
                .ToDictionary(g => g.Key, g => g.OrderBy(f => f.DepartureDatetime).ToList());

            var inconsistencies = new List<string>();

            foreach (var entry in groupedFlights)
            {
                var aircraft = entry.Key;
                var flightList = entry.Value;

                for (int i = 0; i < flightList.Count - 1; i++)
                {
                    var currentFlight = flightList[i];
                    var nextFlight = flightList[i + 1];

                    _logger.LogInformation($"Checking inconsistencies for aircraft {aircraft}");

                    if (currentFlight.ArrivalAirport != nextFlight.DepartureAirport)
                    {
                        inconsistencies.Add(
                            $"Inconsistency for aircraft {aircraft}: " +
                            $"Flight {currentFlight.FlightNumber} arrived at {currentFlight.ArrivalAirport}, " +
                            $"but next flight {nextFlight.FlightNumber} departs from {nextFlight.DepartureAirport}."
                        );
                    }
                }
            }

            return inconsistencies;
        }
    }
}
