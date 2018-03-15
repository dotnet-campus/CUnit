using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MSTest.Extensions.Contracts;
using MSTest.Extensions.Utils;

namespace MSTest.Extensions.Core
{
    /// <summary>
    /// the testmethod proxy, replace the implement of ItestMethod invoke
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
            _testCaseIndex = 0;
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
            TestMethodInitialize(_realSubject);
            var testCases = ContractTest.Method[_realSubject.MethodInfo];
            var result = testCases[_testCaseIndex++].Result;
            TestMethodCleanup(_realSubject);
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
        /// <typeparam name="AttributeType"></typeparam>
        /// <param name="inherit"></param>
        /// <returns></returns>
        public AttributeType[] GetAttributes<AttributeType>(bool inherit) where AttributeType : Attribute
        {
            return _realSubject.GetAttributes<AttributeType>(inherit);
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

        private void TestMethodInitialize([NotNull]ITestMethod testMethod)
        {
            var classInfo = GetClassInfo(testMethod);
            //下面的代码保证第一次跑的时候初始化
            GetBaseTestInitializeMethodsQueue(classInfo);
            GetBaseTestCleanupMethodsQueue(classInfo);
            GetTestInitializeMethod(classInfo);
            GetTestCleanupMethod(classInfo);
            classInfo.SetProperty(MSTestMemberName.TestClassInfoPropertyBaseTestCleanupMethodsQueue, new Queue<MethodInfo>());
            classInfo.SetField(MSTestMemberName.TestClassInfoFieldTestCleanupMethod, null);
            var testCases = ContractTest.Method[testMethod.MethodInfo];
            testCases.Clear();
            testMethod.Invoke(null);
        }

        private void TestMethodCleanup([NotNull]ITestMethod testMethod)
        {
            var classInfo = GetClassInfo(testMethod);
            var baseTestInitializeMethodsQueue = GetBaseTestInitializeMethodsQueue(classInfo);
            var baseTestCleanupMethodsQueue = GetBaseTestCleanupMethodsQueue(classInfo);
            var testInitializeMethod = GetTestInitializeMethod(classInfo);
            var testCleanupMethod = GetTestCleanupMethod(classInfo);
            classInfo.SetProperty(MSTestMemberName.TestClassInfoPropertyBaseTestCleanupMethodsQueue, baseTestCleanupMethodsQueue);
            classInfo.SetField(MSTestMemberName.TestClassInfoFieldTestCleanupMethod, testCleanupMethod);
            classInfo.SetProperty(MSTestMemberName.TestClassInfoPropertyBaseTestInitializeMethodsQueue, new Queue<MethodInfo>());
            classInfo.SetField(MSTestMemberName.TestClassInfoFieldTestInitializeMethod, null);
            testMethod.Invoke(null);
            classInfo.SetProperty(MSTestMemberName.TestClassInfoPropertyBaseTestInitializeMethodsQueue, baseTestInitializeMethodsQueue);
            classInfo.SetField(MSTestMemberName.TestClassInfoFieldTestInitializeMethod, testInitializeMethod);
        }

        private object GetTestCleanupMethod([NotNull]object classInfo)
        {
            if (_testCleanupMethod is null)
            {
                _testCleanupMethod = classInfo.GetField(MSTestMemberName.TestClassInfoFieldTestCleanupMethod);
            }
            return _testCleanupMethod;
        }

        private object GetTestInitializeMethod([NotNull]object classInfo)
        {
            if (_testInitializeMethod is null)
            {
                _testInitializeMethod = classInfo.GetField(MSTestMemberName.TestClassInfoFieldTestInitializeMethod);
            }
            return _testInitializeMethod;
        }

        private object GetBaseTestCleanupMethodsQueue([NotNull]object classInfo)
        {
            if (_baseTestCleanupMethodsQueue is null)
            {
                _baseTestCleanupMethodsQueue = classInfo.GetProperty(MSTestMemberName.TestClassInfoPropertyBaseTestCleanupMethodsQueue);
            }
            return _baseTestCleanupMethodsQueue;
        }

        private object GetClassInfo([NotNull] object testMethod)
        {
            if (_classInfo is null)
            {
                _classInfo = testMethod.GetProperty(MSTestMemberName.TestMethodInfoPropertyParent);
            }
            return _classInfo;
        }

        private object GetBaseTestInitializeMethodsQueue([NotNull] object classInfo)
        {
            if (_baseTestInitializeMethodsQueue is null)
            {
                _baseTestInitializeMethodsQueue = classInfo.GetProperty(MSTestMemberName.TestClassInfoPropertyBaseTestInitializeMethodsQueue);
            }
            return _baseTestInitializeMethodsQueue;
        }


        private readonly ITestMethod _realSubject;
        private int _testCaseIndex;
        private object _classInfo;
        private object _baseTestInitializeMethodsQueue;
        private object _baseTestCleanupMethodsQueue;
        private object _testInitializeMethod;
        private object _testCleanupMethod;
    }
}