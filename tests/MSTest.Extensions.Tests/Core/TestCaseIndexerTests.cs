using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MSTest.Extensions.Core;

#pragma warning disable CS1998

namespace MSTest.Extensions.Tests.Core
{
    [TestClass]
    public class TestCaseIndexerTests
    {
        [TestMethod, SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void ThisMethod_NullArgument_ArgumentNullExceptionThrown()
        {
            // Arrange
            var indexer = new TestCaseIndexer();

            // Action & Assert
            Assert.ThrowsException<ArgumentNullException>(() => indexer[null]);
        }

        private bool _exceptionOccurredInCurrentProperty;

        [TestInitialize, SuppressMessage("ReSharper", "UnusedVariable")]
        public void Current_NotFromTestMethod()
        {
            var indexer = new TestCaseIndexer();
            // Run in TestInitialize, so that current method isn't running in TestMethod.
            try
            {
                var cases = indexer.Current;
            }
            catch (InvalidOperationException)
            {
                // This exception only occurs when Current property is not running in TestMethod.
                _exceptionOccurredInCurrentProperty = true;
            }
        }

        [TestMethod]
        public async Task Current_NotFromTestMethod_InvalidOperationExceptionThrown()
        {
            // Arrange
            // Arrange is transfered into Current_NotFromTestMethod.

            // Action & Assert
            Assert.IsTrue(_exceptionOccurredInCurrentProperty);
        }
    }
}

#pragma warning restore CS1998
