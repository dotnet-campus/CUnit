﻿using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MSTest.Extensions.Contracts;

#pragma warning disable CS1998

namespace MSTest.Extensions.Tests.Contracts
{
    [TestClass]
    public class ContractTestTests
    {
        [TestMethod]
        [DataRow(null, false, false, DisplayName = "If contract is null but action is not null, exception thrown.")]
        [DataRow("", true, false, DisplayName = "If contract is not null but action is null, exception thrown.")]
        [DataRow(null, false, true, DisplayName =
            "If contract is null but async action is not null, exception thrown.")]
        [DataRow("", true, true, DisplayName = "If contract is not null but async action is null, exception thrown.")]
        [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public void Test_NullArgument_ArgumentNullExceptionThrown(string contract, bool isActionNull, bool isAsync)
        {
            if (isAsync)
            {
                // Arrange
                var action = isActionNull ? (Func<Task>) null : async () => { };

                // Action & Assert
                Assert.ThrowsException<ArgumentNullException>(() => { contract.Test(action); });
            }
            else
            {
                // Arrange
                var action = isActionNull ? (Action) null : () => { };

                // Action & Assert
                Assert.ThrowsException<ArgumentNullException>(() => { contract.Test(action); });
            }
        }

        //[TestMethod]
        //public void Test_Action_TestCaseCreatedAndExecuted()
        //{
        //    // Arrange
        //    const string contract = "Test contract description.";
        //    var executed = false;

        //    // Action
        //    contract.Test(() => executed = true);
        //    var result = ContractTest.Method.Current.Single().Result;

        //    // Assert
        //    Assert.AreEqual(result.DisplayName, contract);
        //    Assert.IsTrue(executed);
        //}

        //[TestMethod]
        //public void Test_AsyncAction_TestCaseCreatedAndExecuted()
        //{
        //    // Arrange
        //    const string contract = "Test contract description.";
        //    var executed = false;

        //    // Action
        //    contract.Test(async () =>
        //    {
        //        await Task.Yield();
        //        executed = true;
        //    });
        //    var result = ContractTest.Method.Current.Single().Result;

        //    // Assert
        //    Assert.AreEqual(result.DisplayName, contract);
        //    Assert.IsTrue(executed);
        //}
    }
}

#pragma warning restore CS1998
