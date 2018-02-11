using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MSTest.Extensions.Core
{
    /// <inheritdoc />
    /// <summary>
    /// 表示在被测单元测试方法中发现的用字符串描述契约形式的测试用例。
    /// </summary>
    internal class ContractTestCase : ITestCase
    {
        /// <summary>
        /// 创建一个字符串描述的契约形式的测试用例。
        /// </summary>
        /// <param name="contract">契约描述。</param>
        /// <param name="testCase">测试用例方法。</param>
        internal ContractTestCase(string contract, Action testCase)
        {
            _contract = contract;
            _testCase = () =>
            {
                testCase();
                return Task.CompletedTask;
            };
        }

        /// <summary>
        /// 创建一个字符串描述的契约形式的测试用例。
        /// </summary>
        /// <param name="contract">契约描述。</param>
        /// <param name="testCase">测试用例方法。</param>
        internal ContractTestCase(string contract, Func<Task> testCase)
        {
            _contract = contract;
            _testCase = testCase;
        }

        /// <inheritdoc />
        public TestResult Result => _result ?? (_result = Execute());

        /// <summary>
        /// 执行测试用例并返回测试结果。
        /// </summary>
        /// <returns>单元测试结果。</returns>
        private TestResult Execute()
        {
            // 准备执行环境。
            Exception exception = null;
            var watch = new Stopwatch();
            watch.Start();

            // 执行测试用例。
            try
            {
                _testCase.Invoke().Wait();
            }
            catch (Exception ex)
            {
                exception = ex;
            }
            finally
            {
                watch.Stop();
            }

            // 返回测试结果。
            var result = new TestResult
            {
                DisplayName = _contract,
                Duration = watch.Elapsed,
            };
            if (exception == null)
            {
                result.Outcome = UnitTestOutcome.Passed;
            }
            else
            {
                result.Outcome = UnitTestOutcome.Failed;
                result.TestFailureException = exception;
            }

            return result;
        }

        [ContractPublicPropertyName(nameof(Result))]
        private TestResult _result;

        /// <summary>
        /// 获取测试用例的用例方法。执行此方法即执行 <see cref="_contract"/> 契约的测试。
        /// 返回值一定不为 null，但并不表示它一定是异步的委托。
        /// </summary>
        private readonly Func<Task> _testCase;

        /// <summary>
        /// 获取此契约测试用例的契约描述。
        /// </summary>
        private readonly string _contract;
    }
}
