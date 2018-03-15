using Microsoft.VisualStudio.TestTools.UnitTesting;
using MSTest.Extensions.Contracts;

namespace MSTest.Extensions.Tests.Contracts
{
    [TestClass]
    public class Test
    {
        private int n = 0;
        [Extensions.Contracts.ContractTestCase]
        public void TheMethodNameYouWantToTest()
        {

            "1".Test(() =>
            {
                Assert.AreEqual(n, 1);
            });

            "2".Test(() =>
            {
                Assert.AreEqual(n, 2);
            });

            "3".Test(() =>
            {
                Assert.AreEqual(n, 3);
            });
        }
        [TestInitialize]
        public void init()
        {
            n = n + 1;
        }
    }

    [TestClass]
    public class Test2
    {
        private static int n = 0;
        [Extensions.Contracts.ContractTestCase]
        public void TheMethodNameYouWantToTest()
        {

            "1".Test(() =>
            {
                Assert.AreEqual(n, 1);
            });

            "2".Test(() =>
            {
                Assert.AreEqual(n, 7);
            });

            "3".Test(() =>
            {
                Assert.AreEqual(n, 13);
            });
        }
        [TestInitialize]
        public void init()
        {
            n = n + 1;
        }
        [TestCleanup]
        public void cleanup()
        {
            n = n + 5;
        }
    }
}