using System;
using NUnit.Framework;

namespace QuickSoft.ScanCard.IntegrationTests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
            Assert.Pass();
        }

        [Test]
        public void Test1()
        {
            Assert.Fail("failed");
        }
    }
}