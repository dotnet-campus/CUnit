using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using MSTest.Extensions;
using MSTest.Extensions.Contracts;
using MSTest.Extensions.Core;

// ReSharper disable InconsistentNaming

namespace dotnetCampus.UITest.WPF
{
    /// <summary>
    /// A contract based test case that is discovered from a unit test method.
    /// </summary>
    [PublicAPI]
    public class UIContractTestCaseAttribute : ContractTestCaseAttribute
    {
        private protected override TestMethodProxy CreateTestMethodProxy(ITestMethod testMethod)
        {
            if (!UITestManager.IsInitialized)
            {
                throw new InvalidOperationException("Must call `UITestManager.InitializeApplication` in the Method with AssemblyInitializeAttribute.");
            }

            return new UITestMethodProxy(testMethod);
        }
    }
}
