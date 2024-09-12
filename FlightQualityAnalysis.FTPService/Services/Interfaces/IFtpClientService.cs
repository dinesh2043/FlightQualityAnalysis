
namespace FlightQualityAnalysis.FTPService.Services.Interfaces
{
    public interface IFtpClientService
    {
        Task<Stream> DownloadFile(string remoteFilePath);
        Task Connect();
        Task Disconnect();
    }
}
