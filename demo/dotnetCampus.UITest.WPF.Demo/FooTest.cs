using System.Windows;
using System.Windows.Threading;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using MSTest.Extensions.Contracts;

namespace dotnetCampus.UITest.WPF.Demo
{
    [TestClass]
    public class FooTest
    {
        [AssemblyInitialize]
        public static void InitializeApplication(TestContext testContext)
        {
            UITestManager.InitializeApplication(() =>
            {
                var app = new App();
                app.InitializeComponent();
                app.Run();
            });
        }

        [UIContractTestCase]
        public void TestMainWindow()
        {
            "Test Open MainWindow, MainWindow be opened".Test(() =>
            {
                Assert.AreEqual(Application.Current.Dispatcher, Dispatcher.CurrentDispatcher);
                var mainWindow = new MainWindow();
                bool isMainWindowLoaded = false;
                mainWindow.Loaded += (sender, args) => isMainWindowLoaded = true;
                mainWindow.Show();
                Assert.AreEqual(true, isMainWindowLoaded);
            });

            "Test Close MainWindow, MainWindow be closed".Test(() =>
            {
                var window = Application.Current.MainWindow;
                Assert.AreEqual(true, window is MainWindow);
                bool isMainWindowClosed = false;
                Assert.IsNotNull(window);
                window.Closed += (sender, args) => isMainWindowClosed = true;
                window.Close();
                Assert.AreEqual(true, isMainWindowClosed);
            });
        }
    }
}