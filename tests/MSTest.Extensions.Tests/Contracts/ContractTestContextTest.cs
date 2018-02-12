using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MSTest.Extensions.Contracts;

#pragma warning disable CS1998

namespace MSTest.Extensions.Tests.Contracts
{
    [TestClass]
    public class ContractTestContextTest
    {
        [TestMethod]
        [DataRow(null, false, false, DisplayName = "If contract is null but action is not null, exception thrown.")]
        [DataRow("", true, false, DisplayName = "If contract is not null but action is null, exception thrown.")]
        [DataRow(null, false, true, DisplayName =
            "If contract is null but async action is not null, exception thrown.")]
        [DataRow("", true, true, DisplayName = "If contract is not null but async action is null, exception thrown.")]
        [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void Constructor_NullArgument_ArgumentNullExceptionThrown(string contract, bool isActionNull,
            bool isAsync)
        {
            if (isAsync)
            {
                // Arrange
                var action = isActionNull ? (Func<int, Task>) null : async a => { };

                // Action & Assert
                Assert.ThrowsException<ArgumentNullException>(() => new ContractTestContext<int>(contract, action));
            }
            else
            {
                // Arrange
                var action = isActionNull ? (Action<int>) null : a => { };

                // Action & Assert
                Assert.ThrowsException<ArgumentNullException>(() => new ContractTestContext<int>(contract, action));
            }
        }

        [TestMethod, SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void WithArgument_NullArray_ArgumentNullExceptionThrown()
        {
            // Arrange
            var context = new ContractTestContext<int>("", a => { });

            // Action & Assert
            Assert.ThrowsException<ArgumentNullException>(() => context.WithArguments(null));
        }

        [TestMethod]
        public void WithArgument_EmptyArray_ArgumentExceptionThrown()
        {
            // Arrange
            var context = new ContractTestContext<int>("", a => { });

            // Action & Assert
            Assert.ThrowsException<ArgumentException>(() => context.WithArguments());
        }

        [TestMethod]
        public void WithArgument_OneArgument_TestCaseCreated()
        {
            // Arrange
            var context = new ContractTestContext<int>("", a => { });

            // Action
            context.WithArguments(0);

            // Assert
            var cases = ContractTest.Method.Current;
            Assert.AreEqual(1, cases.Count);
        }

        [TestMethod]
        public void WithArgument_MultipleArgument_TestCaseCreated()
        {
            // Arrange
            var context = new ContractTestContext<int>("", a => { });

            // Action
            context.WithArguments(0, 1);

            // Assert
            var cases = ContractTest.Method.Current;
            Assert.AreEqual(2, cases.Count);
        }

        [TestMethod]
        public void WithArgument_GetUnitTestResult_TestCaseExecuted()
        {
            // Arrange
            var executed = false;
            var context = new ContractTestContext<int>("", a => executed = true);

            // Action
            context.WithArguments(0);
            var result = ContractTest.Method.Current.Single().Result;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(executed);
        }

        [TestMethod]
        public void WithArgument_GetAsyncUnitTestResult_TestCaseExecuted()
        {
            // Arrange
            var executed = false;
            var context = new ContractTestContext<int>("", async a =>
            {
                await Task.Yield();
                executed = true;
            });

            // Action
            context.WithArguments(0);
            var result = ContractTest.Method.Current.Single().Result;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(executed);
        }
    }
}

#pragma warning restore CS1998
