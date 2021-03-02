namespace QuickSoft.ScanCard.Infrastructure.Security
{
    public interface IJwtTokenGenerator
    {
        string CreateToken(string username);
    }
}