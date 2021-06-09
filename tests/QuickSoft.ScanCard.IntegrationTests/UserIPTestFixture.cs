using System;
using System.Net;
using NUnit.Framework;

namespace QuickSoft.ScanCard.IntegrationTests
{
    [TestFixture]
    public class UserIPTestFixture
    {
        [Test]
        public void GetUserIp()
        {
            var externalIpString = new WebClient().DownloadString("https://icanhazip.com").Replace("\\r\\n", "").Trim();
            var externalIp = IPAddress.Parse(externalIpString);

            Console.WriteLine(externalIp.ToString());
        }
    }
}