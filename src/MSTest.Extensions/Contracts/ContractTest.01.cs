using System;
using System.Diagnostics.Contracts;

namespace MSTest.Extensions.Contracts
{
    public static partial class ContractTest
    {
        /// <summary>
        /// 测试此字符串描述的契约。
        /// </summary>
        /// <param name="contract">契约的字符串描述。</param>
        /// <param name="testCase">用于测试此契约的测试用例。</param>
        [System.Diagnostics.Contracts.Pure, NotNull]
        public static ContractTestContext<T> Test<T>([NotNull] this string contract, [NotNull] Action<T> testCase)
        {
            if (contract == null) throw new ArgumentNullException(nameof(contract));
            if (testCase == null) throw new ArgumentNullException(nameof(testCase));

            Contract.EndContractBlock();

            return new ContractTestContext<T>(contract, testCase);
        }
    }
}
