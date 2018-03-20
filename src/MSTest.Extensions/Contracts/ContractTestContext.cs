using System;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
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
            if (testCase == null) throw new ArgumentNullException(nameof(testCase));
#if NET45
#pragma warning disable CS1998 // Async function without await expression
            _testCase = async t => testCase(t);
#pragma warning restore CS1998 // Async function without await expression
#else
            _testCase = t =>
            {
                testCase(t);
                return Task.CompletedTask;
            };
#endif
        }

        /// <summary>
        /// Create a new <see cref="ContractTestContext{T}"/> instance with contract description and async testing action.
        /// </summary>
        /// <param name="contract">The contract description string for this test case.</param>
        /// <param name="testCase">The actual async action to execute the test case.</param>
        public ContractTestContext([NotNull] string contract, [NotNull] Func<T, Task> testCase)
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
        [NotNull, PublicAPI]
#if GENERATED_CODE
        public ContractTestContext<T> WithArguments([NotNull] params T[] ts)
        {
            if (ts == null) throw new ArgumentNullException(nameof(ts));
            if (ts.Length < 1)
#else
        public ContractTestContext<T> WithArguments([CanBeNull] params T[] ts)
        {
            if (ts != null && ts.Length < 1)
#endif
            {
                throw new ArgumentException(
                    $"At least one argument should be passed into test case {_contract}", nameof(ts));
            }
            Contract.EndContractBlock();

#if !GENERATED_CODE
            if (ts == null)
            {
                ts = new T[] { default };
            }
#endif
            // Check if every argument will be formatted into the contract.
            var allFormatted = true;
            for (var i = 0; i < ts.Length; i++)
            {
                allFormatted = _contract.Contains($"{{{i}}}");
                if (!allFormatted) break;
            }

            foreach (var t in ts)
            {
                //For null,the formatted string is Null
                var argumentString = t == null ? "Null" : t.ToString();
                // If any argument is not formatted, post the argument value at the end of the contract string.
                var contract = string.Format(_contract, argumentString);
                if (!allFormatted)
                {
                    contract = contract + $"({argumentString})";
                }

                // Add an argument test case to the test case list.
                ContractTest.Method.Current.Add(new ContractTestCase(contract, () => _testCase(t)));
            }

            return this;
        }

#if GENERATED_CODE
        /// <summary>
        /// When the test case is executed, pass the arguments to the test case action.
        /// </summary>
        /// <returns>The instance itself.</returns>
        [NotNull, PublicAPI]
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
        /// The returning value of this func will never be null, but it does not mean that it is an async func.
        /// </summary>
        [NotNull] private readonly Func<T, Task> _testCase;
    }
}
