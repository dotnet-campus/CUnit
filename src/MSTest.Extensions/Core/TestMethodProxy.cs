using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MSTest.Extensions.Contracts;
using MSTest.Extensions.Utils;

namespace MSTest.Extensions.Core
{
    /// <summary>
    /// the TestMethod proxy, replace the implement of ITestMethod invoke
    /// </summary>
    public class TestMethodProxy : ITestMethod
    {
        /// <summary>
        /// create a TestMethodProxy of an ITestMethod
        /// </summary>
        /// <param name="testMethod"></param>
        public TestMethodProxy([NotNull] ITestMethod testMethod)
        {
            if (testMethod is TestMethodProxy)
            {
                throw new InvalidOperationException("Can not create a TestMethodProxy of another TestMethodProxy");
            }

            _realSubject = testMethod;
            //ReflectionMemberInit();
        }

        /// <summary>
        /// use the TestMethodInitialize and TestMethodCleanup of MsTest
        /// replace the method invoke with ItestCase.Result
        /// </summary>
        /// <param name="arguments"></param>
        /// <returns></returns>
        [NotNull]
        public TestResult Invoke(object[] arguments)
        {
            //TestMethodInitialize();
            //var testCases = ContractTest.Method[_realSubject.MethodInfo];
            //var testCase = testCases[_testCaseIndex++];
            ITestCase testCase = _realSubject.Arguments != null && _realSubject.Arguments.Length > 0
                ? _realSubject.Arguments[0] as ITestCase
                : null;
            if (testCase is null)
            {
                return new TestResult()
                {
                    Outcome = UnitTestOutcome.NotRunnable,
                    LogError = "Can not find a valid test case in the arguments.",
                };
            }

            var result = InvokeCore(testCase);
            //TestMethodCleanup();
            return result;
        }

        public async Task<TestResult> InvokeAsync(object[] arguments)
        {
            ITestCase testCase = _realSubject.Arguments != null && _realSubject.Arguments.Length > 0
                ? _realSubject.Arguments[0] as ITestCase
                : null;
            if (testCase is null)
            {
                return new TestResult()
                {
                    Outcome = UnitTestOutcome.NotRunnable,
                    LogError = "Can not find a valid test case in the arguments.",
                };
            }

            var result = await InvokeCoreAsync(testCase);
            //TestMethodCleanup();
            return result;
        }

        [NotNull]
        [ItemNotNull]
        internal async Task<TestResult> InvokeAsync()
        {
            //TestMethodInitialize();
            //var testCases = ContractTest.Method[_realSubject.MethodInfo];
            //var testCase = testCases[_testCaseIndex++];
            ITestCase testCase = _realSubject.Arguments != null && _realSubject.Arguments.Length > 0
                ? _realSubject.Arguments[0] as ITestCase
                : null;
            if (testCase is null)
            {
                return new TestResult()
                {
                    Outcome = UnitTestOutcome.NotRunnable,
                    LogError = "Can not find a valid test case in the arguments.",
                };
            }
            var result = await InvokeCoreAsync(testCase).ConfigureAwait(false);
            //TestMethodCleanup();
            return result;
        }

        [NotNull]
        private protected virtual TestResult InvokeCore([NotNull] ITestCase testCase)
        {
            var result = testCase.Result;
            return result;
        }

        [NotNull]
        [ItemNotNull]
        private protected virtual async Task<TestResult> InvokeCoreAsync([NotNull] ITestCase testCase)
        {
            TestResult result;
            if (testCase is ContractTestCase contractTestCase)
            {
                result = await contractTestCase.ExecuteAsync().ConfigureAwait(false);
            }
            else
            {
                result = testCase.Result;
            }

            return result;
        }

        /// <summary>
        /// return real instance implement of GetAllAttributes
        /// </summary>
        /// <param name="inherit"></param>
        /// <returns></returns>
        public Attribute[] GetAllAttributes(bool inherit)
        {
            return _realSubject.GetAllAttributes(inherit);
        }

        /// <summary>
        /// return real instance implement of GetAttributes
        /// </summary>
        /// <typeparam name="TAttributeType"></typeparam>
        /// <param name="inherit"></param>
        /// <returns></returns>
        public TAttributeType[] GetAttributes<TAttributeType>(bool inherit) where TAttributeType : Attribute
        {
            return _realSubject.GetAttributes<TAttributeType>(inherit);
        }

        /// <summary>
        /// return real instance implement of TestMethodName
        /// </summary>
        public string TestMethodName => _realSubject.TestMethodName;

        /// <summary>
        /// return real instance implement of TestClassName
        /// </summary>
        public string TestClassName => _realSubject.TestClassName;

        /// <summary>
        /// return real instance implement of ReturnType
        /// </summary>
        public Type ReturnType => _realSubject.ReturnType;

        /// <summary>
        /// return real instance implement of Arguments
        /// </summary>
        public object[] Arguments => _realSubject.Arguments;

        /// <summary>
        /// return real instance implement of ParameterTypes
        /// </summary>
        public ParameterInfo[] ParameterTypes => _realSubject.ParameterTypes;

        /// <summary>
        /// return real instance implement of MethodInfo
        /// </summary>
        public MethodInfo MethodInfo => _realSubject.MethodInfo;


        ///// <summary>
        ///// extract the test TestMethodInitialize of _realSubject and run
        ///// </summary>
        //private void TestMethodInitialize()
        //{
        //    ClassInfo.SetProperty(MSTestMemberName.TestClassInfoPropertyBaseTestCleanupMethodsQueue,
        //        new Queue<MethodInfo>());
        //    ClassInfo.SetField(MSTestMemberName.TestClassInfoFieldTestCleanupMethod, null);
        //    // 现在改成直接传递 TestCase 作为参数，不需要再清理列表了，不会相互干扰
        //    //var testCases = ContractTest.Method[_realSubject.MethodInfo];
        //    //testCases.Clear();
        //    _realSubject.Invoke(null);
        //}

        ///// <summary>
        ///// extract the test TestMethodCleanup of _realSubject and run
        ///// </summary>
        //private void TestMethodCleanup()
        //{
        //    ClassInfo.SetProperty(MSTestMemberName.TestClassInfoPropertyBaseTestCleanupMethodsQueue,
        //        BaseTestCleanupMethodsQueue);
        //    ClassInfo.SetField(MSTestMemberName.TestClassInfoFieldTestCleanupMethod, TestCleanupMethod);
        //    ClassInfo.SetProperty(MSTestMemberName.TestClassInfoPropertyBaseTestInitializeMethodsQueue,
        //        new Queue<MethodInfo>());
        //    ClassInfo.SetField(MSTestMemberName.TestClassInfoFieldTestInitializeMethod, null);
        //    _realSubject.Invoke(null);
        //    ClassInfo.SetProperty(MSTestMemberName.TestClassInfoPropertyBaseTestInitializeMethodsQueue,
        //        BaseTestInitializeMethodsQueue);
        //    ClassInfo.SetField(MSTestMemberName.TestClassInfoFieldTestInitializeMethod, TestInitializeMethod);
        //}

        ///// <summary>
        ///// get the nonpublic memeber value of _realsubject var Reflection
        ///// </summary>
        //private void ReflectionMemberInit()
        //{
        //    ClassInfo = _realSubject.GetProperty(MSTestMemberName.TestMethodInfoPropertyParent);
        //    TestCleanupMethod = (MethodInfo)ClassInfo
        //        .GetField(MSTestMemberName.TestClassInfoFieldTestCleanupMethod);
        //    TestInitializeMethod = (MethodInfo)ClassInfo
        //        .GetField(MSTestMemberName.TestClassInfoFieldTestInitializeMethod);
        //    BaseTestInitializeMethodsQueue = (Queue<MethodInfo>)ClassInfo
        //        .GetProperty(MSTestMemberName.TestClassInfoPropertyBaseTestInitializeMethodsQueue);
        //    BaseTestCleanupMethodsQueue = (Queue<MethodInfo>)ClassInfo
        //        .GetProperty(MSTestMemberName.TestClassInfoPropertyBaseTestCleanupMethodsQueue);
        //}

        //private object ClassInfo { get; set; }

        //private Queue<MethodInfo> BaseTestInitializeMethodsQueue { get; set; }

        //private Queue<MethodInfo> BaseTestCleanupMethodsQueue { get; set; }

        //private MethodInfo TestInitializeMethod { get; set; }

        //private MethodInfo TestCleanupMethod { get; set; }
        private readonly ITestMethod _realSubject;
    }
}