using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MSTest.Extensions.Core;

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
            var classInfo = GetProperty(testMethod, "Parent");
            var baseTestInitializeMethodsQueue = GetProperty(classInfo, "BaseTestInitializeMethodsQueue");
            var baseTestCleanupMethodsQueue = GetProperty(classInfo, "BaseTestCleanupMethodsQueue");
            var testInitializeMethod = GetField(classInfo, "testInitializeMethod");
            var testCleanupMethod = GetField(classInfo, "testCleanupMethod");
            SetProperty(classInfo, "BaseTestCleanupMethodsQueue", new Queue<MethodInfo>());
            SetField(classInfo, "testCleanupMethod", null);
            var testCases = ContractTest.Method[testMethod.MethodInfo];
            testCases.Clear();
            testMethod.Invoke(null);
            //var testCases = new List<ITestCase>(ContractTest.Method[testMethod.MethodInfo]);
            var result = testCases[_testCaseIndex++].Result;
            SetProperty(classInfo, "BaseTestCleanupMethodsQueue", baseTestCleanupMethodsQueue);
            SetField(classInfo, "testCleanupMethod", testCleanupMethod);
            SetProperty(classInfo, "BaseTestInitializeMethodsQueue", new Queue<MethodInfo>());
            SetField(classInfo, "testInitializeMethod", null);
            testMethod.Invoke(null);
            SetProperty(classInfo, "BaseTestInitializeMethodsQueue", baseTestInitializeMethodsQueue);
            SetField(classInfo, "testInitializeMethod", testInitializeMethod);
            return new[] { result };
        }


        public object GetField(object source, string propertyName)
        {
            var type = source.GetType();
            var field = type.GetField(propertyName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            return field.GetValue(source);
        }

        public object GetProperty(object source, string propertyName)
        {
            var type = source.GetType();
            var property = type.GetProperty(propertyName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            return property.GetValue(source);
        }

        public void SetField(object target, string propertyName, object value)
        {
            var type = target.GetType();
            var field = type.GetField(propertyName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            field.SetValue(target, value);

        }

        public void SetProperty(object target, string propertyName, object value)
        {
            var type = target.GetType();
            var property = type.GetProperty(propertyName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            property.SetValue(target, value);

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
            Contract.EndContractBlock();

            // Collect all test cases from the target unit test method.
            Collect(methodInfo);

            var cases = ContractTest.Method[methodInfo];
            VerifyContracts(cases);

            return Enumerable.Range(0, cases.Count).Select(x => (object[]) null);
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
            return ContractTest.Method[methodInfo][_testCaseIndex++].DisplayName;
        }

        #endregion

        #region Collect and verify test cases

        [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
        private static void Collect([NotNull] MethodInfo methodInfo)
        {
            var type = methodInfo.DeclaringType;
            Contract.Requires(type != null,
                "The method must be declared in a type. If this exception happened, there might be a bug in MSTest v2.");

            var testInstance = Activator.CreateInstance(type);
            var testCaseList = ContractTest.Method[methodInfo];
            try
            {
                // Invoke target test method to collect all test cases.
                methodInfo.Invoke(testInstance, null);
            }
            catch (TargetInvocationException ex)
            {
                // An exception has occurred during the test cases collecting.
                // We should create a new test result to report this exception instead of reporting the collected ones.
                var exception = ex.InnerException;
                Contract.Assume(exception != null);

                testCaseList.Clear();
                testCaseList.Add(new ReadonlyTestCase(exception, exception.GetType().FullName));
            }
            catch (Exception ex)
            {
                // An unexpected exception has occurred, but we don't know why it happens.
                // We should Add a new test result to report this exception.
                testCaseList.Add(new ReadonlyTestCase(ex, ex.GetType().FullName));
            }

            if (!testCaseList.Any())
            {
                testCaseList.Add(new ReadonlyTestCase(UnitTestOutcome.Inconclusive,
                    @"No test found",
                    @"A unit test method should contain at least one test case.
Try to call Test extension method to collect one.

```csharp
""Test contract description"".Test(() =>
{
    // Arrange
    // Action
    // Assert
});
```

If you only need to write a normal test method, use `TestMethodAttribute` instead of `ContractTestCaseAttribute`."));
            }
        }

        /// <summary>
        /// Find out the test cases which have the same contract string, add a special exception to it.
        /// </summary>
        /// <param name="cases">The test cases of a single test method.</param>
        private void VerifyContracts([NotNull] IList<ITestCase> cases)
        {
            var caseContractSet = new HashSet<string>();
            var duplicatedCases = new HashSet<ITestCase>();
            var duplicatedContracts = new HashSet<string>();

            foreach (var @case in cases)
            {
                if (caseContractSet.Contains(@case.DisplayName))
                {
                    duplicatedCases.Add(@case);
                    duplicatedContracts.Add(@case.DisplayName);
                }
                else
                {
                    caseContractSet.Add(@case.DisplayName);
                }
            }

            foreach (var @case in duplicatedCases)
            {
                cases.Remove(@case);
            }

            foreach (var contract in duplicatedContracts)
            {
                cases.Add(new ReadonlyTestCase(UnitTestOutcome.Error, contract,
                    $@"Duplicated Contract String

Two or more test cases have the same contract string which is ""{contract}"".
1. Please check whether you have created two test cases which have the same contract string. Notice that different formatted string may become the same string when the placeholders are filled with arguments.
2. If you are using formatted strings but all the value strings are the same(by calling argument.ToString()), please avoid using the formatted contract string. "));
            }

            // throw new NotImplementedException();
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
