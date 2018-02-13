using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MSTest.Extensions.Contracts;

namespace MSTest.Extensions.Tests.Contracts
{
    [TestClass]
    public class ContractTestCaseAttributeTest
    {
        [ContractTestCase]
        public void RunAPassedTestCase()
        {
            "".Test(() => { });
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
