using System;
using System.Threading;
using System.Windows.Threading;

// ReSharper disable InconsistentNaming

namespace dotnetCampus.UITest.WPF
{
    public static class UITestManager
    {
        public static void InitializeApplication(Action runApplication)
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
                var dispatcher = Dispatcher.CurrentDispatcher;
                dispatcher.InvokeAsync(() =>
                {
                    manualResetEvent.Set();
                });

                runApplication();
            });

            thread.SetApartmentState(ApartmentState.STA);
            thread.IsBackground = true;
            thread.Start();

            manualResetEvent.WaitOne();

            IsInitialized = true;
        }

        private static bool _initializing;
        internal static bool IsInitialized { private set; get;  }
    }
}