using CsvHelper;
using FlightQualityAnalysis.FTPService.Model;
using FlightQualityAnalysis.FTPService.Services.Interfaces;
using System.Globalization;

namespace FlightQualityAnalysis.FTPService.Services
{
    public class CsvFlightInfoParser : ICsvFlightInfoParser
    {
        public IEnumerable<FlightInfo> ParseCsv(Stream csvStream)
        {
            using (var reader = new StreamReader(csvStream))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                // Register mapping
                csv.Context.RegisterClassMap<FlightInfoMap>();
                // Parse and return records
                return csv.GetRecords<FlightInfo>().ToList();
            }
        }
    }
}
