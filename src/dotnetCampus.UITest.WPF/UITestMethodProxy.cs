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
                    var result = await contractTestCase.ExecuteAsync()
                        // 如果在执行过程，应用退出了，那在没有加上 ConfigureAwait 设置为 false 那将需要调度回 UI 线程，才能返回
                        // 由于应用退出了，也就是 UI 线程不会调度的任务
                        // 因此不会返回，单元测试将会卡住
                        .ConfigureAwait(false);
                    return result;
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