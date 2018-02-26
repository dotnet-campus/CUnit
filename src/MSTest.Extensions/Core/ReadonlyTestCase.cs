using System;
using System.Diagnostics.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MSTest.Extensions.Core
{
    /// <inheritdoc />
    /// <summary>
    /// Stores a specific unit test result.
    /// </summary>
    internal class ReadonlyTestCase : ITestCase
    {
        /// <summary>
        /// Initialize a new instance of <see cref="ReadonlyTestCase"/> to report a specific exception.
        /// </summary>
        /// <param name="exception">The exception that should be reported as a unit test result.</param>
        /// <param name="displayName">The display name that would be displayed to report the exception.</param>
        public ReadonlyTestCase([NotNull] Exception exception, [NotNull] string displayName)
        {
            if (exception == null) throw new ArgumentNullException(nameof(exception));
            DisplayName = displayName ?? throw new ArgumentNullException(nameof(displayName));

            Contract.EndContractBlock();

            Result = new TestResult
            {
                Outcome = UnitTestOutcome.Error,
                TestFailureException = exception,
                DisplayName = displayName,
            };
        }

        /// <summary>
        /// Initialize a new instance of <see cref="ReadonlyTestCase"/> to report a specific exception.
        /// </summary>
        /// <param name="notRunnableName">The name that will be displayed when the test case is not runnable.</param>
        /// <param name="notRunnableReason">The reason why this test case is not runnable.</param>
        internal ReadonlyTestCase([NotNull] string notRunnableName, [NotNull] string notRunnableReason)
        {
            if (notRunnableReason == null) throw new ArgumentNullException(nameof(notRunnableReason));
            DisplayName = notRunnableName ?? throw new ArgumentNullException(nameof(notRunnableName));

            Contract.EndContractBlock();

            Result = new TestResult
            {
                Outcome = UnitTestOutcome.NotRunnable,
                DisplayName = notRunnableName,
                TestContextMessages = notRunnableReason,
            };
        }

        /// <inheritdoc />
        public string DisplayName { get; }

        /// <inheritdoc />
        /// <summary>
        /// Get the specific unit test result.
        /// </summary>
        public TestResult Result { get; }
    }
}
