using System;
using CryptSharp;
using NUnit.Framework;

namespace QuickSoft.ScanCard.IntegrationTests
{
    [TestFixture]
    public class UserPasswordTestFixture
    {
        [Test]
        public void PasswordGeneratorTest()
        {
            // var pass = Crypter.Blowfish.Crypt("123");
            var pass = "$2y$10$Tk5C/YWnC4xK8pRPIRbuyOM8rSrE47oIhX0jAFMS8puTiwrryzjMO";
            Console.WriteLine(pass);
            var val = Crypter.CheckPassword("123", "$2y$10$Tk5C/YWnC4xK8pRPIRbuyOM8rSrE47oIhX0jAFMS8puTiwrryzjMO");
            Console.WriteLine(val);

            var salt = BCrypt.Net.BCrypt.GenerateSalt(11, 'y');
            var hash = BCrypt.Net.BCrypt.HashPassword("123", salt);
            Console.WriteLine(hash);

            var isValid = BCrypt.Net.BCrypt.Verify("123", hash);
            Assert.True(isValid);

            var hash1 = "$2y$11$mgOXrAqimsdccSKt.CZGbOl4o6LMO3HQOI6V.TG8lR5zNSH7D8loq";
            var hash2 = "$2y$11$2S6yjpMmJRg9//SfmIMNg.SWEHoLkvXNZIkrdn5IzoalK2S80fwVW";
            var hash3 = "$2y$11$SaEAg8auDEHirM3dFXLFQO9Mb3booVT6AuHXe.WvYa0u23J6gSazi";

            Assert.True(Crypter.CheckPassword("123", hash3));
        }
    }
}