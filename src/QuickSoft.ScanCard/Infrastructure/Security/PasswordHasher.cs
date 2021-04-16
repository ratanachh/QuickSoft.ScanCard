using CryptSharp;

namespace QuickSoft.ScanCard.Infrastructure.Security
{
    public class PasswordHasher : IPasswordHasher
    {
        public string Hash(string password)
        {
            return Crypter.Blowfish.Crypt(password);
        }
        
        public bool Verify(string password, string encryptedPassword)
        {
            return Crypter.CheckPassword(password, encryptedPassword);
        }
    }
}