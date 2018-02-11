using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MSTest.Extensions.Contracts
{
    /// <summary>
    /// 允许在单元测试方法中以 “契约.Test(测试用例)” 的形式来描述测试用例并执行单元测试方法。<para/>
    /// 方法内的写法为：<para/>
    /// <code>
    /// "契约 1".Test(() => { 测试用例 });<para/>
    /// "契约 2".Test(() =><para/>
    /// {<para/>
    ///     // 测试用例<para/>
    /// });<para/>
    /// "契约 3".Test(async () => { 测试用例 });<para/>
    /// </code>
    /// </summary>
    public sealed class ContractTestCaseAttribute : TestMethodAttribute, ITestDataSource
    {
        // ## 工作原理
        // 
        // 1. 当单元测试框架发现了一个标记为 TestMethodAttribute 的方法之后，会搜索实现了 ITestDataSource 接口的 Attribute。
        // 1. 继承自 ITestDataSource 的实例 #1 被创建，GetData 方法被调用：
        //     - 直接执行一次被测方法；
        //     - 收集被测方法内所有的 “契约.Test(测试用例)” 的调用；
        //     - 根据收集到的所有的测试用例个数返回相应个数的空数组给框架。
        // 1. 继承自 TestMethodAttribute 的实例 #2 被创建。
        // 1. 实例 #2 的 Execute 方法和实例 #1 的 GetDisplayName 方法被交替调用：
        //     - Execute 依次取出一个测试用例，并执行；
        //     - GetDisplayName 依次取出一个测试用例，并获取显示名称。

        /// <summary>
        /// 每一次 <see cref="Execute"/> 或 <see cref="ITestDataSource.GetDisplayName"/> 方法被执行，
        /// 都需要从此字段中获取当前正在执行的测试用例序号。
        /// 由于两个方法分别由两个不同的实例执行，所以两个方法内部都需要独自进行自增。
        /// </summary>
        private int _testCaseIndex;

        #region TestMethodAttribute 的实现实例

        /// <inheritdoc />
        public override TestResult[] Execute(ITestMethod testMethod)
        {
            var contractTestCases = ContractTest.Method[testMethod.MethodInfo];
            var result = contractTestCases[_testCaseIndex++].Result;
            return new[] { result };
        }

        #endregion

        #region ITestDataSource

        /// <summary>
        /// 当被测方法准备执行时，此方法会被调用以获取测试用例。
        /// </summary>
        /// <param name="methodInfo">准备执行的被测方法。</param>
        /// <returns>被测方法执行时需要的参数列表。这里永远返回 null 数组，因为不需要参数。</returns>
        IEnumerable<object[]> ITestDataSource.GetData(MethodInfo methodInfo)
        {
            methodInfo.Invoke(Activator.CreateInstance(methodInfo.DeclaringType), null);
            var testCaseList = ContractTest.Method[methodInfo];
            return Enumerable.Range(0, testCaseList.Count).Select(x => (object[])null);
        }

        /// <summary>
        /// 每一次执行被测方法后，此方法都会被调用以获取此次被测的测试用例名称。
        /// </summary>
        /// <param name="methodInfo">刚刚执行的被测方法。</param>
        /// <param name="data">刚刚执行被测方法时使用的参数列表。</param>
        /// <returns>此次测试用例的显示名称。</returns>
        string ITestDataSource.GetDisplayName(MethodInfo methodInfo, object[] data)
        {
            return ContractTest.Method[methodInfo][_testCaseIndex++].Result.DisplayName;
        }

        #endregion
    }
}
