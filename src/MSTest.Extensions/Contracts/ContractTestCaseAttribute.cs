using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

// ## How it works?
// 
// 1. When the MSTestv2 discover a method which is marked with a `TestMethodAttribute`,
//    it will search all `Attributes` to find out the one which derived from `ITestDataSource`.
// 1. The first instance (#1) of `ContractTestCaseAttribute` will be created because it derived from `ITestDataSource`.
// 1. `GetMethod` of #1 is called:
//     - Invoke the target unit test method.
//     - Collect all test cases that are created during the target unit test method invoking.
//     - Return an empty array with length equals to test case count to the MSTestv2 framework.
//     - *In this case, `Execute` would be called the same times to that array length.*
// 1. The second instance (#2) of `ContractTestCaseAttribute` will be created because it derived from `TestMethodAttribute`.
// 1. `Execute` of #2 and `GetDisplayName` of #1 will be called alternately by the MSTestv2 framework:
//     - When `Execute` is called, fetch a test case and run it to get the test result.
//     - When `GetDisplayName` is called, fetch a test case and get the contract description string from it.

namespace MSTest.Extensions.Contracts
{
    /// <summary>
    /// Enable the unit test writing style of `"contract string".Test(TestCaseAction)`.
    /// </summary>
    [PublicAPI]
    public sealed class ContractTestCaseAttribute : TestMethodAttribute, ITestDataSource
    {
        #region Instance derived from TestMethodAttribute

        /// <inheritdoc />
        [NotNull]
        public override TestResult[] Execute([NotNull] ITestMethod testMethod)
        {
            var contractTestCases = ContractTest.Method[testMethod.MethodInfo];
            var result = contractTestCases[_testCaseIndex++].Result;
            return new[] { result };
        }

        #endregion

        #region Instance derived from ITestDataSource

        /// <summary>
        /// When a unit test method is preparing to run, This method will be called
        /// to fetch all the test cases of target unit test method.
        /// </summary>
        /// <param name="methodInfo">Target unit test method.</param>
        /// <returns>
        /// The parameter array which will be passed into the target unit test method.
        /// We don't need any parameter, so we return an all null array with length equals to test case count.
        /// </returns>
        [NotNull]
        public IEnumerable<object[]> GetData([NotNull] MethodInfo methodInfo)
        {
            var type = methodInfo.DeclaringType;
            if (type == null)
            {
                throw new ArgumentException(
                    "The method must be declared in a type. " +
                    "If this exception happened, there might be a bug in MSTest v2.",
                    nameof(methodInfo));
            }

            methodInfo.Invoke(Activator.CreateInstance(type), null);
            var testCaseList = ContractTest.Method[methodInfo];
            return Enumerable.Range(0, testCaseList.Count).Select(x => (object[])null);
        }

        /// <summary>
        /// Each time after <see cref="Execute"/> is called, this method will be called
        /// to retrieve the display name of the test case.
        /// </summary>
        /// <param name="methodInfo">Target unit test method.</param>
        /// <param name="data">The parameter list which was returned by <see cref="GetData"/>.</param>
        /// <returns>The display name of this test case.</returns>
        [NotNull]
        public string GetDisplayName([NotNull] MethodInfo methodInfo, [NotNull] object[] data)
        {
            return ContractTest.Method[methodInfo][_testCaseIndex++].Result.DisplayName;
        }

        #endregion

        /// <summary>
        /// Gets or increment the current test case index.
        /// <see cref="Execute"/> and <see cref="GetDisplayName"/> should increment it separately
        /// because they are not in the same instance.
        /// </summary>
        private int _testCaseIndex;
    }
}
