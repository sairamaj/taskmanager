using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using Autofac;
using TaskManager.Repository;
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
                var builder = new ContainerBuilder();
                new TaskRepository().InitializeTaskModules(builder).Wait();
                builder.RegisterModule(new TaskModule());
                var serviceLocator = ServiceLocatorFactory.Create(builder);
                var win = new MainWindow(serviceLocator.Resolve<ICommandTreeItemViewMapper>())
                {
                    DataContext = new MainViewModel(
                        serviceLocator,
                        serviceLocator.Resolve<ICommandTreeItemViewMapper>(), 
                        serviceLocator.Resolve<ITaskRepository>())
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
