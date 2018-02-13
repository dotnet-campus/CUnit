using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MSTest.Extensions.Contracts;

namespace MSTest.Extensions.Tests.Contracts
{
    [TestClass]
    public class ContractTestCaseAttributeTest
    {
        [ContractTestCase, SuppressMessage("ReSharper", "InconsistentNaming")]
        public void LetTheMSTestv2RunThisExtensionOnce()
        {
            "".Test(() => { });
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
