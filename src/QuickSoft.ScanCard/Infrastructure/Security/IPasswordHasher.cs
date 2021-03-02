using System.Threading.Tasks;

namespace QuickSoft.ScanCard.Infrastructure.Security
{
    public interface IPasswordHasher
    {
        string Hash(string password);
        bool Verify(string password, string cryptedPassword);
    }
}