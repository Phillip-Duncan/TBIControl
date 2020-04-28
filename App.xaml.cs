using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace TBIControl
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        public App():base()
        {
            this.Dispatcher.UnhandledException += OnDispatcherUnhandledException;
        }

        void OnDispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show("Unhandled exception occurred: \n" + e.Exception.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);

            TBIControl.Properties.Settings.Default.pmdport = "";
            TBIControl.Properties.Settings.Default.pmdconnected = false;
            TBIControl.Properties.Settings.Default.pmdserobj = null;

            TBIControl.Properties.Settings.Default.elecport = "";
            TBIControl.Properties.Settings.Default.elecconnected = false;
            TBIControl.Properties.Settings.Default.elecserobj = null;

            TBIControl.Properties.Settings.Default.Save();

        }

            private void Application_Exit(object sender, ExitEventArgs e)
        {
            TBIControl.Properties.Settings.Default.pmdport = "";
            TBIControl.Properties.Settings.Default.pmdconnected = false;
            TBIControl.Properties.Settings.Default.pmdserobj = null;

            TBIControl.Properties.Settings.Default.elecport = "";
            TBIControl.Properties.Settings.Default.elecconnected = false;
            TBIControl.Properties.Settings.Default.elecserobj = null;

            TBIControl.Properties.Settings.Default.Save();

        }

        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            TBIControl.Properties.Settings.Default.pmdport = "";
            TBIControl.Properties.Settings.Default.pmdconnected = false;
            TBIControl.Properties.Settings.Default.pmdserobj = null;

            TBIControl.Properties.Settings.Default.elecport = "";
            TBIControl.Properties.Settings.Default.elecconnected = false;
            TBIControl.Properties.Settings.Default.elecserobj = null;

            TBIControl.Properties.Settings.Default.Save();
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {

        }


    }
}
