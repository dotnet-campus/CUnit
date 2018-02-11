using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MSTest.Extensions.Contracts;

namespace MSTest.Extensions.Core
{
    /// <summary>
    /// 包含所有单元测试方法中的所有测试用例信息。
    /// </summary>
    internal class TestCaseIndexer
    {
        /// <summary>
        /// 获取某个测试方法中包含的所有测试用例。
        /// </summary>
        /// <param name="method">单元测试被测方法描述符。</param>
        /// <returns>单元测试方法内部记录的所有测试用例列表，如果尚未开始发现，则返回空列表。</returns>
        internal IList<ITestCase> this[MethodInfo method] => this[GetKey(method)];

        /// <summary>
        /// 获取当前调用堆栈中测试方法所包含的所有测试用例。
        /// </summary>
        internal IList<ITestCase> Current => this[GetCurrentTestMethod()];

        /// <summary>
        /// 获取某个测试方法描述符包含的所有测试用例。
        /// </summary>
        /// <param name="testKey">单元测试被测方法描述符。</param>
        /// <returns>单元测试方法内部记录的所有测试用例列表，如果尚未开始发现，则返回空列表。</returns>
        private IList<ITestCase> this[string testKey]
        {
            get
            {
                if (!_testCaseDictionary.TryGetValue(testKey, out var list))
                {
                    list = new List<ITestCase>();
                    _testCaseDictionary[testKey] = list;
                }

                return list;
            }
        }

        /// <summary>
        /// 记录被测单元测试方法内部发现的所有测试用例。
        /// Key 为 “命名空间.被测类名.被测方法名”，可通过 <see cref="GetKey"/> 得到；
        /// Value 为特定被测方法内发现的所有测试用例的列表，通过 <see cref="ContractTest.Method"/> 属性获取可避免获取到 null。
        /// </summary>
        private readonly Dictionary<string, List<ITestCase>> _testCaseDictionary =
            new Dictionary<string, List<ITestCase>>();

        /// <summary>
        /// 获取某个成员的单元测试描述符。
        /// </summary>
        /// <param name="member">成员。</param>
        /// <returns>单元测试描述符。</returns>
        private static string GetKey(MemberInfo member)
        {
            return $"{member.DeclaringType.FullName}.{member.Name}";
        }

        /// <summary>
        /// 在当前调用堆栈中查找单元测试方法。
        /// </summary>
        /// <returns>找到的单元测试方法。</returns>
        private static MethodInfo GetCurrentTestMethod()
        {
            var stackTrace = new StackTrace();
            for (var i = 0; i < stackTrace.FrameCount; i++)
            {
                var method = stackTrace.GetFrame(i).GetMethod();
                if (method.GetCustomAttribute<TestMethodAttribute>() != null)
                {
                    return (MethodInfo) method;
                }
            }

            throw new InvalidOperationException("当前调用堆栈中不存在被测方法。");
        }
    }
}
