using FlightQualityAnalysis.FTPService.Services.Interfaces;
using FluentFTP;

namespace FlightQualityAnalysis.FTPService.Services
{
    public class FtpClientService : IFtpClientService
    {
        private FtpClient _client;

        public FtpClientService(FtpClient client)
        {
            _client = client;
        }

        public void Connect()
        {
            if (!_client.IsConnected)
            {
                _client.Connect();
            }
        }

        public void Disconnect()
        {
            if (_client.IsConnected)
            {
                _client.Disconnect();
            }
        }

        public Stream DownloadFile(string remoteFilePath)
        {
            var memoryStream = new MemoryStream();
            _client.DownloadStream(memoryStream, remoteFilePath);
            memoryStream.Position = 0;  // Reset stream position for reading
            return memoryStream;
        }
    }
}
