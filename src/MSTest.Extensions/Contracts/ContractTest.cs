using System;
using System.Threading.Tasks;
using MSTest.Extensions.Core;

namespace MSTest.Extensions.Contracts
{
    /// <summary>
    /// 包含辅助契约形式测试用例编写的辅助方法。
    /// </summary>
    public static partial class ContractTest
    {
        #region 形式 1： "契约".Test(() => { 测试用例 })

        /// <summary>
        /// 测试此字符串描述的契约。
        /// </summary>
        /// <param name="contract">契约的字符串描述。</param>
        /// <param name="testCase">用于测试此契约的测试用例。</param>
        public static void Test(this string contract, Action testCase) =>
            Method.Current.Add(new ContractTestCase(contract, testCase));

        /// <summary>
        /// 测试此字符串描述的契约。
        /// </summary>
        /// <param name="contract">契约的字符串描述。</param>
        /// <param name="testCase">用于测试此契约的测试用例。</param>
        public static void Test(this string contract, Func<Task> testCase) =>
            Method.Current.Add(new ContractTestCase(contract, testCase));

        #endregion

        #region 形式 2： await "契约" 测试用例

        ///// <summary>
        ///// 将此字符串作为契约，将 await 之后的逻辑代码作为测试此契约的测试用例。
        ///// </summary>
        ///// <param name="contract">契约的字符串描述。</param>
        ///// <returns></returns>
        //[EditorBrowsable(EditorBrowsableState.Never)]
        //public static IAwaiter GetAwaiter(this string contract)
        //{
        //    var result = new TestCaseAwaitable(contract);
        //    Method.Current.Add(result);
        //    return result;
        //}

        #endregion

        /// <summary>
        /// 获取所有单元测试方法中的所有测试用例信息。
        /// </summary>
        internal static TestCaseIndexer Method { get; } = new TestCaseIndexer();
    }
}
