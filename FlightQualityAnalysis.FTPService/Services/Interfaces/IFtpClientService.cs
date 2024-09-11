using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightQualityAnalysis.FTPService.Services.Interfaces
{
    public interface IFtpClientService
    {
        Stream DownloadFile(string remoteFilePath);
        void Connect();
        void Disconnect();
    }
}
