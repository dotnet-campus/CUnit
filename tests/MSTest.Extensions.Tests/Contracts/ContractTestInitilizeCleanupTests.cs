using Microsoft.VisualStudio.TestTools.UnitTesting;
using MSTest.Extensions.Contracts;

namespace MSTest.Extensions.Tests.Contracts
{
    // 此初始化方式暂无对应方式，先注释掉
    //[TestClass]
    //public class ContractTestInitilizeTests
    //{
    //    private static int _staticField = 0;
    //    [ContractTestCase]
    //    public void TestInitializeAttribute()
    //    {
    //        "the 1st time TestInitialize run 1 time,_staticField==1".Test(() =>
    //        {
    //            Assert.AreEqual(_staticField, 1);
    //        });

    //        "the 2ed time TestInitialize run 2 times,_staticField==2".Test(() =>
    //        {
    //            Assert.AreEqual(_staticField, 2);
    //        });

    //        "the 3rd time TestInitialize run 3 times,_staticField==2".Test(() =>
    //        {
    //            Assert.AreEqual(_staticField, 3);
    //        });
    //    }
    //    [TestInitialize]
    //    public void Init()
    //    {
    //        _staticField = _staticField + 1;
    //    }
    //}

    // 此清理方式暂无对应方式，先注释掉
    //[TestClass]
    //public class ContractTestCleanupTests
    //{
    //    private static int _staticField = 0;
    //    [ContractTestCase]
    //    public void TheMethodNameYouWantToTest()
    //    {
    //        "the 1st time TestCleanup not run,_staticField==1".Test(() =>
    //        {
    //            Assert.AreEqual(_staticField, 0);
    //        });

    //        "the 2ed time TestCleanup not run 1 time,_staticField==5".Test(() =>
    //        {
    //            Assert.AreEqual(_staticField, 5);
    //        });

    //        "the 3rd time TestCleanup not run run 2 times,_staticField==10".Test(() =>
    //        {
    //            Assert.AreEqual(_staticField, 10);
    //        });
    //    }
        
    //    [TestCleanup]
    //    public void Cleanup()
    //    {
    //        _staticField = _staticField + 5;
    //    }
    //}

    // 此初始化方式暂无对应方式，先注释掉
    //[TestClass]
    //public class ContractTestInitilizeCleanupTests
    //{
    //    private static int _staticField = 0;
    //    [ContractTestCase]
    //    public void TestInitializeAttribute()
    //    {
    //        "the 1st time TestInitialize run 1 time,TestCleanup not run,_staticField==1".Test(() =>
    //        {
    //            Assert.AreEqual(_staticField, 1);
    //        });

    //        "the 2ed time TestInitialize run 2 times,TestCleanup not run 1 time,_staticField==7".Test(() =>
    //        {
    //            Assert.AreEqual(_staticField, 7);
    //        });

    //        "the 3rd time TestInitialize run 3 times,TestCleanup not run run 2 times,_staticField==13".Test(() =>
    //        {
    //            Assert.AreEqual(_staticField, 13);
    //        });
    //    }
    //    [TestInitialize]
    //    public void Init()
    //    {
    //        _staticField = _staticField + 1;
    //    }
    //    [TestCleanup]
    //    public void Cleanup()
    //    {
    //        _staticField = _staticField + 5;
    //    }
    //}
}