using System;

namespace MSTest.Extensions.Contracts
{
    public static partial class ContractTest
    {
        /// <summary>
        /// 测试此字符串描述的契约。
        /// </summary>
        /// <param name="contract">契约的字符串描述。</param>
        /// <param name="testCase">用于测试此契约的测试用例。</param>
        public static ContractTestContext<T> Test<T>(this string contract, Action<T> testCase)
        {
            return new ContractTestContext<T>(contract, testCase);
        }
    }
}
