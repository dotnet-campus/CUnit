using System;

namespace MSTest.Extensions.CustomTestManagers
{
    /// <summary>
    /// The test exception result
    /// </summary>
    public class TestExceptionResult
    {
        internal TestExceptionResult(string displayName, Exception exception)
        {
            DisplayName = displayName;
            Exception = exception;
        }

        /// <summary>
        /// The test display name
        /// </summary>
        public string DisplayName { get; }

        /// <summary>
        /// The test exception
        /// </summary>
        public Exception Exception { get; }
    }
}