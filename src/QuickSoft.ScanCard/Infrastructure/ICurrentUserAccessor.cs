using Shyjus.BrowserDetection.Browsers;

namespace QuickSoft.ScanCard.Infrastructure
{
    public interface ICurrentUserAccessor
    {
        string GetCurrentUsername();
        string GetCurrentUserType();

        IBrowser GetUserAgent();
        string GetUserIp();
        string GetAuditId();
    }
}