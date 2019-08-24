using System;
using System.Diagnostics;
using System.Windows;
using TaskManager.ViewModels;
using Utils.Core;
using Utils.Core.Extensions;

namespace TaskManager
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            this.DispatcherUnhandledException += (s, ex) =>
            {
                Trace.WriteLine(ex.Exception.GetExceptionDetails());
                MessageBox.Show(ex.Exception.GetExceptionDetails());
                System.Environment.Exit(-1);
            };

            try
            {
                var serviceLocator = ServiceLocatorFactory.Get();
                var win = new MainWindow(serviceLocator.Resolve<ICommandTreeItemViewMapper>())
                {
                    DataContext = new MainViewModel(serviceLocator.Resolve<ICommandTreeItemViewMapper>())
                };

                win.Show();
            }
            catch (Exception ex1)
            {
                MessageBox.Show(ex1.GetExceptionDetails());
                System.Environment.Exit(-1);
            }
        }
    }
}
