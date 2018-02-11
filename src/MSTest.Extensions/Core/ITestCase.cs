using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MSTest.Extensions.Core
{
    /// <summary>
    /// 表示在被测单元测试方法中发现的一个测试用例。
    /// </summary>
    internal interface ITestCase
    {
        /// <summary>
        /// 获取此测试用例的测试结果。
        /// </summary>
        TestResult Result { get; }
    }
}
