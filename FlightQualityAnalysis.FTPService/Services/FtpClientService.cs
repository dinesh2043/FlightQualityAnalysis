using FlightQualityAnalysis.FTPService.Services.Interfaces;
using FluentFTP;
using Microsoft.Extensions.Logging;

namespace FlightQualityAnalysis.FTPService.Services
{
    public class FtpClientService : IFtpClientService
    {
        private AsyncFtpClient _client;
        private ILogger<FtpClientService> _logger;

        public FtpClientService(AsyncFtpClient client, ILogger<FtpClientService> logger)
        {
            _client = client;
            _logger = logger;
        }

        public async Task Connect()
        {
            if (!_client.IsConnected)
            {
                _logger.LogInformation("FTP Server Connected.");
                await _client.Connect();
            }
        }

        public async Task Disconnect()
        {
            if (_client.IsConnected)
            {
                _logger.LogInformation("FTP Server Disconnected.");
                await _client.Disconnect();
            }
        }

        public async Task<Stream> DownloadFile(string remoteFilePath)
        {
            var memoryStream = new MemoryStream();
            await _client.DownloadStream(memoryStream, remoteFilePath);
            // Reset stream position for reading
            memoryStream.Position = 0;
            _logger.LogInformation("File Stream Downloaded.");
            return memoryStream;
        }
    }
}
