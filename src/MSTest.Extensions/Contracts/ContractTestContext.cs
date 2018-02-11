using System;
using MSTest.Extensions.Core;

namespace MSTest.Extensions.Contracts
{
    public class ContractTestContext<T>
    {
        private readonly string _contract;
        private readonly Action<T> _testCase;

        public ContractTestContext(string contract, Action<T> testCase)
        {
            _contract = contract;
            _testCase = testCase;
        }

        public ContractTestContext<T> WithArguments(params T[] ts)
        {
            foreach (var t in ts)
            {
                ContractTest.Method.Current.Add(new ContractTestCase(
                    string.Format(_contract, t), () => _testCase(t)));
            }

            return this;
        }

#if GENERATED_CODE
        public ContractTestContext<T> WithArguments(T t)
        {
            ContractTest.Method.Current.Add(new ContractTestCase(
                string.Format(_contract, t), () => _testCase(t)));
            return this;
        }
#endif
    }
}
