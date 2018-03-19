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
    public class ContractTestCaseAttributeTests
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

        [ContractTestCase,Ignore]
        public void WithArguments_WithSameContractString()
        {
            "This is the same string.".Test(() => { });
            "This is the same string.".Test(() => { });
        }

        [ContractTestCase]
        public void WithArguments_WithFormatString()
        {
            "If {0}, then {1}.".Test((string condition, string result) =>
            {
                // Test Case.
            }).WithArguments(("A", "A'"), ("B", "B'"));
        }

        [ContractTestCase]
        public void WithArguments_WithPartialFormatString()
        {
            "If {0}, then something...".Test((string condition, string result) =>
            {
                // Test Case.
            }).WithArguments(("A", "A'"), ("B", "B'"));
        }

        [ContractTestCase]
        public void WithArguments_WithoutFormatString()
        {
            "If something..., then something others...".Test((string condition, string result) =>
            {
                // Test Case.
            }).WithArguments(("A", "A'"), ("B", "B'"));
        }

        [TestMethod, Ignore]
        public void OriginalAssertButFailed()
        {
            Assert.AreEqual(5, 6);
        }

        [ContractTestCase, Ignore]
        public void AssertButFailed()
        {
            "This test case will always fail.".Test(() => Assert.AreEqual(5, 6));
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
