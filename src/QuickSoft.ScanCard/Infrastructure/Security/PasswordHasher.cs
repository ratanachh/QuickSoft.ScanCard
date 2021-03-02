using CryptSharp;

namespace QuickSoft.ScanCard.Infrastructure.Security
{
    public class PasswordHasher : IPasswordHasher
    {
        public string Hash(string password)
        {
            return Crypter.Blowfish.Crypt(password);
        }
        
        public bool Verify(string password, string cryptedPassword)
        {
            return Crypter.CheckPassword(password, cryptedPassword);
        }
    }
}