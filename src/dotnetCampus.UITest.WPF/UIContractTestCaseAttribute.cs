using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using MSTest.Extensions;
using MSTest.Extensions.Contracts;
using MSTest.Extensions.Core;

// ReSharper disable InconsistentNaming

namespace dotnetCampus.UITest.WPF
{
    public static class UITestManager
    {
        public static void InitializeApplication(Application application)
        {
            if (_initializing)
            {
                throw new InvalidOperationException($"Do not call InitializeApplication twice.");
            }

            _initializing = true;

            var manualResetEvent = new ManualResetEvent(false);
            var thread = new Thread(() =>
            {
                Thread.CurrentThread.Name = "UIThread";
                application.Startup += (sender, args) =>
                {
                    manualResetEvent.Set();
                };

                application.Run();
            });

            thread.SetApartmentState(ApartmentState.MTA);
            thread.IsBackground = true;
            thread.Start();

            manualResetEvent.WaitOne();

            IsInitialized = true;
        }

        private static bool _initializing;
        internal static bool IsInitialized { private set; get;  }
    }

    /// <summary>
    /// A contract based test case that is discovered from a unit test method.
    /// </summary>
    [PublicAPI]
    public class UIContractTestCaseAttribute : ContractTestCaseAttribute
    {
        private protected override TestMethodProxy CreateTestMethodProxy(ITestMethod testMethod)
        {
            if (!UITestManager.IsInitialized)
            {
                throw new InvalidOperationException("Must call `UITestManager.InitializeApplication` in the Method with AssemblyInitializeAttribute.");
            }

            return new UITestMethodProxy(testMethod);
        }
    }

    class UITestMethodProxy : TestMethodProxy
    {
        public UITestMethodProxy([NotNull] ITestMethod testMethod) : base(testMethod)
        {
        }

        private protected override TestResult InvokeCore(ITestCase testCase)
        {
            return Application.Current.Dispatcher.Invoke(() => base.InvokeCore(testCase));
        }

        private protected override async Task<TestResult> InvokeCoreAsync(ITestCase testCase)
        {
            return await await Application.Current.Dispatcher.InvokeAsync(async () => await base.InvokeCoreAsync(testCase));
        }
    }
}
