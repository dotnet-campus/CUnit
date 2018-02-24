using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MSTest.Extensions.Contracts;

namespace MSTest.Extensions.Tests.Contracts
{
    /// <summary>
    /// All methods in this class are not real unit test.
    /// It is only used for you to check the test result in the result view.
    /// </summary>
    [TestClass]
    public class ContractTestCaseAttributeTest
    {
        [ContractTestCase]
        public void RunAPassedTestCase()
        {
            "".Test(() => { });
        }

        [ContractTestCase]
        public void RunATestCaseWithExtraInfo()
        {
            "Output".Test(() => Console.WriteLine("This is a test message."));
            "Error".Test(() => Console.Error.WriteLine("This is an error message."));
        }


        [TestMethod, Ignore]
        public void OriginalAssertButFailed()
        {
            Assert.AreEqual(5, 6);
        }

        [ContractTestCase, Ignore]
        public void AssertButFailed()
        {
            "This test case will always fail.".Test(async () => Assert.AreEqual(5, 6));
        }

        [ContractTestCase, Ignore]
        public void RunAFailedTestCase()
        {
            "".Test(() => Assert.Fail());
        }

        [ContractTestCase, Ignore]
        public void NoTestCasesIsDefinedInTheTestCaseMethod()
        {
        }

        [ContractTestCase, Ignore]
        public void ThrowExceptionDirectlyInTheTestCaseMethod()
        {
            throw new InvalidOperationException();
        }
    }
}
