using System;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

// ReSharper disable InconsistentNaming

namespace dotnetCampus.UITest.WPF
{
    public static class UITestManager
    {
        public static void InitializeApplication(Func<Application> applicationCreator)
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

                var application = applicationCreator();
                var type = application.GetType();
                var resourceAssembly = type.Assembly;

                typeof(Application).GetField("_resourceAssembly", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static)!.SetValue(null, resourceAssembly);

                var methodInfo = type.GetMethod("InitializeComponent", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                if (methodInfo != null)
                {
                    methodInfo.Invoke(application, new object[0]);
                }

                application.Startup += (sender, args) =>
                {
                    manualResetEvent.Set();
                };

                application.Run();
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