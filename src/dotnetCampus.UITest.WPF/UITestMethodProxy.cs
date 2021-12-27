using System.Threading.Tasks;
using System.Windows;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using MSTest.Extensions;
using MSTest.Extensions.Core;

// ReSharper disable InconsistentNaming

namespace dotnetCampus.UITest.WPF
{
    class UITestMethodProxy : TestMethodProxy
    {
        public UITestMethodProxy([NotNull] ITestMethod testMethod) : base(testMethod)
        {
        }

        private protected override TestResult InvokeCore(ITestCase testCase)
        {
            if (testCase is ContractTestCase contractTestCase)
            {
                var task = Application.Current.Dispatcher.Invoke(async () =>
                {
                    return await contractTestCase.ExecuteAsync();
                });

                return task.Result;
            }

            return base.InvokeCore(testCase);
        }

        private protected override async Task<TestResult> InvokeCoreAsync(ITestCase testCase)
        {
            return await await Application.Current.Dispatcher.InvokeAsync(async () => await base.InvokeCoreAsync(testCase));
        }
    }
}