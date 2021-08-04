using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MSTest.Extensions.Contracts;

namespace MSTest.Extensions.CustomTestManagers
{
    /// <summary>
    /// The custom test manager which can run without MSTest
    /// </summary>
    public class CustomTestManager
    {
        /// <summary>
        /// Create the custom test manager
        /// </summary>
        public CustomTestManager()
        {
            Context = new CustomTestManagerRunContext();
        }

        /// <summary>
        /// Create the custom test manager
        /// </summary>
        /// <param name="context"></param>
        public CustomTestManager(CustomTestManagerRunContext context)
        {
            Context = context;
        }

        /// <summary>
        /// Run all the test from MethodInfo
        /// </summary>
        /// <param name="methodInfo"></param>
        /// <returns></returns>
        [NotNull]
        public TestManagerRunResult Run([NotNull] MethodInfo methodInfo)
        {
            var exceptionList = new List<TestExceptionResult>();
            int count = 0;
            TimeSpan duration = TimeSpan.Zero;

            var contractTestCaseAttribute = methodInfo.GetCustomAttribute<ContractTestCaseAttribute>();
            if (contractTestCaseAttribute != null)
            {
                // 获取执行次数
                foreach (var data in contractTestCaseAttribute.GetData(methodInfo))
                {
                    count++;
                    var displayName = contractTestCaseAttribute.GetDisplayName(methodInfo, data);

                    try
                    {
                        var resultList = contractTestCaseAttribute.Execute(new FakeTestMethod(methodInfo));
                        var result = resultList[0];
                        duration += result.Duration;
                    }
#pragma warning disable CA1031 // 不捕获常规异常类型
                    catch (Exception e)
#pragma warning restore CA1031 // 不捕获常规异常类型
                    {
                        exceptionList.Add(new TestExceptionResult(displayName, e));
                    }
                }
            }

            return new TestManagerRunResult(count, duration, exceptionList);
        }

        /// <summary>
        /// Run all the test from TestClass
        /// </summary>
        /// <returns></returns>
        [NotNull]
        public TestManagerRunResult Run([NotNull] Type testClass)
        {
            var exceptionList = new List<TestExceptionResult>();
            int count = 0;

            TimeSpan duration = TimeSpan.Zero;

            foreach (var methodInfo in testClass.GetMethods())
            {
                var testManagerRunResult = Run(methodInfo);

                count += testManagerRunResult.AllTestCount;
                duration += testManagerRunResult.Duration;
                exceptionList.AddRange(testManagerRunResult.TestExceptionResultList);
            }

            return new TestManagerRunResult(count, duration, exceptionList);
        }

        /// <summary>
        /// Run all the test in assembly
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        [NotNull]
        public TestManagerRunResult Run(Assembly assembly = null)
        {
            assembly ??= Assembly.GetCallingAssembly();
            var exceptionList = new List<TestExceptionResult>();
            int count = 0;

            TimeSpan duration = TimeSpan.Zero;

            foreach (var type in assembly.GetTypes())
            {
                if (type.GetCustomAttribute<TestClassAttribute>() != null)
                {
                    var testManagerRunResult = Run(type);

                    count += testManagerRunResult.AllTestCount;
                    duration += testManagerRunResult.Duration;
                    exceptionList.AddRange(testManagerRunResult.TestExceptionResultList);
                }
            }

            return new TestManagerRunResult(count, duration, exceptionList);
        }

        private CustomTestManagerRunContext Context { get; }

        class FakeTestMethod : ITestMethod
        {
            public FakeTestMethod(MethodInfo methodInfo, [CanBeNull] object obj = null)
            {
                MethodInfo = methodInfo;
                Obj = obj ?? Activator.CreateInstance(methodInfo.DeclaringType!);
            }

            [NotNull]
            public TestResult Invoke(object[] arguments)
            {
                MethodInfo.Invoke(Obj, arguments);
                return new TestResult();
            }

            [NotNull]
            public Attribute[] GetAllAttributes(bool inherit)
            {
                return MethodInfo.GetCustomAttributes().ToArray();
            }

            [NotNull]
            public TAttributeType[] GetAttributes<TAttributeType>(bool inherit) where TAttributeType : Attribute
            {
                return MethodInfo.GetCustomAttributes().OfType<TAttributeType>().ToArray();
            }

            private object Obj { get; }
            [NotNull]
            public string TestMethodName => MethodInfo.Name;
            [NotNull]
            public string TestClassName => MethodInfo.DeclaringType?.Name ?? string.Empty;
            [CanBeNull]
            public Type ReturnType => MethodInfo.ReturnType;
            public object[] Arguments { set; get; }
            [CanBeNull]
            public ParameterInfo[] ParameterTypes => MethodInfo.GetParameters();
            public MethodInfo MethodInfo { set; get; }

            public FakeTestInfo Parent { get; } = new FakeTestInfo();
        }

        class FakeTestInfo
        {
            // 命名不能更改，框架内使用反射获取

            private MethodInfo testCleanupMethod;
            private MethodInfo testInitializeMethod;

            public Queue<MethodInfo> BaseTestInitializeMethodsQueue { set; get; }

            public Queue<MethodInfo> BaseTestCleanupMethodsQueue { set; get; }
        }
    }
}
