using CsvHelper;
using FlightQualityAnalysis.FTPService.Model;
using FlightQualityAnalysis.FTPService.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System.Globalization;

namespace FlightQualityAnalysis.FTPService.Services
{
    public class CsvFlightInfoParser : ICsvFlightInfoParser
    {
        private readonly ILogger<CsvFlightInfoParser> _logger;

        public CsvFlightInfoParser(ILogger<CsvFlightInfoParser> logger)
        {
            _logger = logger;
        }
        public async Task<IEnumerable<FlightInfo>> ParseCsv(Stream csvStream)
        {
            using (var reader = new StreamReader(csvStream))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                // Register mapping
                csv.Context.RegisterClassMap<FlightInfoMap>();

                // Parse and return records asynchronously
                var records = new List<FlightInfo>();

                await foreach (var record in csv.GetRecordsAsync<FlightInfo>())
                {
                    records.Add(record);
                }

                _logger.LogInformation("Csv File parsed successfully.");

                return records;
            }
        }
    }
}
