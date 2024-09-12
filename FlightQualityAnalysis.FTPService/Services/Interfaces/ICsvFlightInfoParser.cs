using FlightQualityAnalysis.FTPService.Model;

namespace FlightQualityAnalysis.FTPService.Services.Interfaces
{
    public interface ICsvFlightInfoParser
    {
        Task<IEnumerable<FlightInfo>> ParseCsv(Stream csvStream);
    }
}
