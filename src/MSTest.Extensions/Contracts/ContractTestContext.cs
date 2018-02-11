using System;
using System.Diagnostics.Contracts;
using MSTest.Extensions.Core;

namespace MSTest.Extensions.Contracts
{
    /// <summary>
    /// Provides builder for a particular contracted test case.
    /// </summary>
    public class ContractTestContext<T>
    {
        /// <summary>
        /// Create a new <see cref="ContractTestContext{T}"/> instance with contract description and testing action.
        /// </summary>
        /// <param name="contract">The contract description string for this test case.</param>
        /// <param name="testCase">The actual action to execute the test case.</param>
        public ContractTestContext([NotNull] string contract, [NotNull] Action<T> testCase)
        {
            _contract = contract ?? throw new ArgumentNullException(nameof(contract));
            _testCase = testCase ?? throw new ArgumentNullException(nameof(testCase));
        }

        /// <summary>
        /// When the test case is executed, pass the argument(s) to the test case action.
        /// </summary>
        /// <param name="ts">The argument(s) that will be passed into the test case action.</param>
        /// <returns>The instance itself.</returns>
        /// <remarks>
        /// Note that we only verify the <paramref name="ts"/> argument in runtime. In this case, we have the power to pass an array instead of writing them all in a method parameter list.
        /// </remarks>
        [NotNull]
        public ContractTestContext<T> WithArguments([NotNull] params T[] ts)
        {
            if (ts == null) throw new ArgumentNullException(nameof(ts));
            if (ts.Length == 1)
            {
                throw new ArgumentException(
                    $"At least one argument should be passed into test case {_contract}", nameof(ts));
            }

            Contract.EndContractBlock();

            foreach (var t in ts)
            {
                ContractTest.Method.Current.Add(new ContractTestCase(
                    string.Format(_contract, t), () => _testCase(t)));
            }

            return this;
        }

#if GENERATED_CODE
        /// <summary>
        /// When the test case is executed, pass the arguments to the test case action.
        /// </summary>
        /// <returns>The instance itself.</returns>
        public ContractTestContext<T> WithArguments(T t)
        {
            ContractTest.Method.Current.Add(new ContractTestCase(
                string.Format(_contract, t), () => _testCase(t)));
            return this;
        }
#endif

        /// <summary>
        /// Gets the contract description string for this test case.
        /// </summary>
        [NotNull] private readonly string _contract;

        /// <summary>
        /// Invoke this action to execute this test case with the argument(s).
        /// </summary>
        [NotNull] private readonly Action<T> _testCase;
    }
}
