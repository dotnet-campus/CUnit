using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MSTest.Extensions.Core;

#pragma warning disable CS1998

namespace MSTest.Extensions.Tests.Core
{
    [TestClass]
    public class ContractTestCaseTests
    {
        [TestMethod]
        [DataRow(null, false, false, DisplayName = "If contract is null but action is not null, exception thrown.")]
        [DataRow("", true, false, DisplayName = "If contract is not null but action is null, exception thrown.")]
        [DataRow(null, false, true, DisplayName = "If contract is null but async action is not null, exception thrown.")]
        [DataRow("", true, true, DisplayName = "If contract is not null but async action is null, exception thrown.")]
        [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void Constructor_NullArgument_ArgumentNullExceptionThrown(string contract, bool isActionNull, bool isAsync)
        {
            if (isAsync)
            {
                // Arrange
                var action = isActionNull ? (Func<Task>) null : async () => { };

                // Action & Assert
                Assert.ThrowsException<ArgumentNullException>(() => new ContractTestCase(contract, action));
            }
            else
            {
                // Arrange
                var action = isActionNull ? (Action) null : () => { };

                // Action & Assert
                Assert.ThrowsException<ArgumentNullException>(() => new ContractTestCase(contract, action));
            }
        }

        [TestMethod]
        public void Execute_NothingDone_Passed()
        {
            // Arrange
            var @case = new ContractTestCase("", () => { });

            // Action
            var result = @case.Result;

            // Assert
            Assert.AreEqual(result.Outcome, UnitTestOutcome.Passed);
        }

        [TestMethod]
        public void Execute_Wait_Passed()
        {
            // Arrange
            var waitTime = TimeSpan.FromMilliseconds(10);
            var @case = new ContractTestCase("", () => Thread.Sleep(waitTime));

            // Action
            var result = @case.Result;

            // Assert
            Assert.IsTrue(result.Duration >= waitTime);
        }

        [TestMethod]
        public void Execute_CaseThrowsException_Failed()
        {
            // Arrange
            var @case = new ContractTestCase("", () => throw new InvalidOperationException());

            // Action
            var result = @case.Result;

            // Assert
            Assert.AreEqual(result.Outcome, UnitTestOutcome.Failed);
            Assert.IsTrue(result.TestFailureException is InvalidOperationException);
        }

        [TestMethod]
        public void Execute_ConsoleWriteLine_OutputCollected()
        {
            // Assert
            const string output = "This is a test message.";
            var @case = new ContractTestCase("", () => Console.Write(output));

            // Action
            var result = @case.Result;

            // Assert
            Assert.AreEqual(result.LogOutput, output);
        }

        [TestMethod]
        public void Execute_ErrorWriteLine_ErrorCollected()
        {
            // Assert
            const string error = "This is a error message.";
            var @case = new ContractTestCase("", () => Console.Error.Write(error));

            // Action
            var result = @case.Result;

            // Assert
            Assert.AreEqual(result.LogError, error);
        }
    }
}

#pragma warning restore CS1998
