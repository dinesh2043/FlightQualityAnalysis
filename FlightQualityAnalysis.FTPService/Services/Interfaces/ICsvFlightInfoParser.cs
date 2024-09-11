using FlightQualityAnalysis.FTPService.Model;

namespace FlightQualityAnalysis.FTPService.Services.Interfaces
{
    public interface ICsvFlightInfoParser
    {
        IEnumerable<FlightInfo> ParseCsv(Stream csvStream);
    }
}
