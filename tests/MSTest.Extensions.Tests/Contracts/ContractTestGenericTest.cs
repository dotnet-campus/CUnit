using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MSTest.Extensions.Contracts;

#pragma warning disable CS1998

namespace MSTest.Extensions.Tests.Contracts
{
    [TestClass]
    public class ContractTestGenericTest
    {
        [TestMethod]
        [DataRow(null, false, false, DisplayName = "If contract is null but action is not null, exception thrown.")]
        [DataRow("", true, false, DisplayName = "If contract is not null but action is null, exception thrown.")]
        [DataRow(null, false, true, DisplayName = "If contract is null but async action is not null, exception thrown.")]
        [DataRow("", true, true, DisplayName = "If contract is not null but async action is null, exception thrown.")]
        [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void Test_NullArgument_ArgumentNullExceptionThrown(string contract, bool isActionNull, bool isAsync)
        {
            if (isAsync)
            {
                // Arrange
                var action = isActionNull ? (Func<int, Task>)null : async a => { };

                // Action & Assert
                Assert.ThrowsException<ArgumentNullException>(() => contract.Test(action));
            }
            else
            {
                // Arrange
                var action = isActionNull ? (Action<int>)null : a => { };

                // Action & Assert
                Assert.ThrowsException<ArgumentNullException>(() => contract.Test(action));
            }
        }

        [TestMethod]
        public void Test_GenericAction_ContractTestContextCreated()
        {
            // Arrange & Action
            var context = "".Test((int a) => { });

            // Assert
            Assert.IsNotNull(context);
        }

        [TestMethod]
        public void Test_AsyncGenericAction_ContractTestContextCreated()
        {
            // Arrange & Action
            var context = "".Test(async (int a) => { });

            // Assert
            Assert.IsNotNull(context);
        }
    }
}

#pragma warning restore CS1998
