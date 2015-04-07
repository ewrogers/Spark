using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
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
            var viewModel = new MainWindowViewModel(App.ApplicationName);

            // Bind the request close event to closing the window
            viewModel.RequestClose += delegate
            {
                window.Close();
            };

            // Assign the view model to the data context and display the main window
            window.DataContext = viewModel;
            window.Show();
        }
    }
}
