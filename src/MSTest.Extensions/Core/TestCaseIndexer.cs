using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Reflection;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using MSTest.Extensions.Contracts;

namespace MSTest.Extensions.Core
{
    /// <summary>
    /// Contains all test case information of all test unit method which are discovered by <see cref="ContractTestCaseAttribute"/>.
    /// </summary>
    internal class TestCaseIndexer
    {
        /// <summary>
        /// Gets all test cases of a specified method.
        /// </summary>
        /// <param name="method">The target unit test method.</param>
        /// <returns>
        /// The test case list of the specified unit test method. If the discovery is not started, then it returns an empty list.
        /// </returns>
        [NotNull]
        internal IReadOnlyList<ITestCase> this[[NotNull] MethodInfo method]
        {
            get
            {
                if (method == null) throw new ArgumentNullException(nameof(method));
                Contract.EndContractBlock();

                return _testCaseDictionary[method];
            }
        }

        /// <summary>
        /// Gets all test cases of current unit test method. This method is found through the stack trace.
        /// </summary>
        [NotNull]
        internal IReadOnlyList<ITestCase> Current => this[_currentTestMethod??throw new InvalidOperationException()];

        internal void SetCurrentCollection([NotNull] MethodInfo testMethod, [NotNull] List<ITestCase> testCaseList)
        {
            if (testMethod == null)
            {
                throw new ArgumentNullException(nameof(testMethod));
            }
            if (testCaseList == null)
            {
                throw new ArgumentNullException(nameof(testCaseList));
            }
            Contract.EndContractBlock();

            _currentTestMethod = testMethod;

            _testCaseDictionary[testMethod] = testCaseList;
        }

        [CanBeNull]
        private MethodInfo _currentTestMethod;

        /// <summary>
        /// Stores all the test cases that discovered or will be discovered from all unit test methods.
        /// Key: Test Method
        /// Value: All test cases of a specified method. You can get rid of null value by calling Indexer.
        /// </summary>
        [NotNull]
        private readonly Dictionary<MethodInfo, List<ITestCase>> _testCaseDictionary =
            new Dictionary<MethodInfo, List<ITestCase>>();

        internal void AddToCurrent([NotNull] ITestCase testCase)
        {
            if (testCase == null)
            {
                throw new ArgumentNullException(nameof(testCase));
            }

            if (_currentTestMethod is null)
            {
                throw new InvalidOperationException();
            }
            Contract.EndContractBlock();

            _testCaseDictionary[_currentTestMethod].Add(testCase);
        }

        // 由于 MSTest 加了一层代理类，导致无法通过堆栈找到实际的单元测试方法
        // 即 MSTest 的调用顺序大概如此：
        // RunTestMethodAsync
        //  -> 调用实际的单元测试方法
        // GetDisplayName
        //  -> 调进 GetCurrentTestMethod 方法
        // 于是从调用堆栈就无法让我们找到实际的单元测试方法了
        ///// <summary>
        ///// Find the unit test method through current stack trace.
        ///// </summary>
        ///// <returns>The unit test method that found from stack trace.</returns>
        //[NotNull]
        //private static MethodInfo GetCurrentTestMethod()
        //{
        //    var stackTrace = new StackTrace();
        //    string methodName = string.Empty;
        //    for (var i = 0; i < stackTrace.FrameCount; i++)
        //    {
        //        var method = stackTrace.GetFrame(i).GetMethod();
        //         methodName = method.Name;
        //         Log(methodName);
        //        if (method.GetCustomAttribute<TestMethodAttribute>() != null ||
        //            method.GetCustomAttribute<ContractTestCaseAttribute>() != null)
        //        {
        //            return (MethodInfo)method;
        //        }
        //    }

        //    throw new InvalidOperationException(
        //        "There is no unit test method in the current stack trace. " +
        //        "This method should only be called directly or indirectly from the unit test method."+ methodName);
        //}

        private static void Log(string message) => Trace.WriteLine(message);
    }
}
