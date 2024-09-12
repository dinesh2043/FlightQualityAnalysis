using FlightQualityAnalysis.FTPService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightQualityAnalysis.FTPService.Services.Interfaces
{
    public interface IFlightInfoService
    {
        Task<IEnumerable<FlightInfo>> ReadCsvWithFluentFtpAndCsvHelper(string remoteFilePath);
    }
}
