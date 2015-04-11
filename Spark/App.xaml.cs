using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Deployment.Application;
using System.Reflection;
using System.Windows;

using Spark.ViewModels;
using Spark.Views;

namespace Spark
{
    public partial class App : Application
    {
        public static readonly string ApplicationName = "Spark";

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Initialize the main window and view model
            var window = new MainWindow();
            var viewModel = new MainViewModel(App.ApplicationName);

            // Bind the request close event to closing the window
            viewModel.RequestClose += delegate
            {
                window.Close();
            };

            // Assign the view model to the data context and display the main window
            window.DataContext = viewModel;
            window.Show();
        }

        public static Version GetRunningVersion()
        {
            if (ApplicationDeployment.IsNetworkDeployed)
                return ApplicationDeployment.CurrentDeployment.CurrentVersion;

            return Assembly.GetExecutingAssembly().GetName().Version;
        }
    }
}
