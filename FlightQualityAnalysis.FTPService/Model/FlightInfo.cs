
namespace FlightQualityAnalysis.FTPService.Model
{
    public class FlightInfo
    {
        public int Id { get; set; }
        public required string AircraftRegistrationNumber { get; set; }
        public string? AircraftType { get; set; }
        public string? FlightNumber { get; set; }
        public string? DepartureAirport { get; set; }
        public DateTime? DepartureDatetime { get; set; }
        public string? ArrivalAirport { get; set; }
        public DateTime? ArrivalDatetime { get; set; }
    }
}
