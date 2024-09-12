using FlightQualityAnalysis.FTPService.Model;

namespace FlightQualityAnalysis.FTPService.Services.Interfaces
{
    public interface IFlightInconsistencyChecker
    {
        List<string> CheckForInconsistencies(IEnumerable<FlightInfo> flightInfo);
    }
}
