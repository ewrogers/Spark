using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Deployment.Application;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows;

using Spark.Models;
using Spark.ViewModels;
using Spark.Views;

namespace Spark
{
    public partial class App : Application
    {
        public static readonly string ApplicationName = "Spark";
        public static readonly string SettingsFileName = "Settings.xml";

        public static UserSettings CurrentSettings { get; protected set; }

        #region Application Lifecycle
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Create default settings
            App.CurrentSettings = LoadSettingsOrDefaults(SettingsFileName);

            // Initialize the main window and view model
            var window = new MainWindow();
            var viewModel = new MainViewModel(App.CurrentSettings);

            // Bind the request close event to closing the window
            viewModel.RequestClose += delegate
            {
                window.Close();
            };

            // Assign the view model to the data context and display the main window
            window.DataContext = viewModel;
            window.Show();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            SaveUserSettings(App.CurrentSettings, App.SettingsFileName);
            base.OnExit(e);
        }
        #endregion

        #region Save/Load User Settings
        static void SaveUserSettings(UserSettings settings, string filename)
        {
            try
            {
                // Persist user settings to file
                App.CurrentSettings.SaveToFile(SettingsFileName);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Unable to save user settings: {0}", ex.Message);
            }
        }

        static UserSettings LoadSettingsOrDefaults(string filename)
        {
            var workingDirectory = Environment.CurrentDirectory;

            try
            {
                // Load user settings from file
                if (File.Exists(SettingsFileName))
                    return UserSettings.LoadFromFile(filename);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Unable to load user settings: {0}", ex.Message);
            }

            return UserSettings.CreateDefaults();
        }
        #endregion

        public static Version GetRunningVersion()
        {
            if (ApplicationDeployment.IsNetworkDeployed)
                return ApplicationDeployment.CurrentDeployment.CurrentVersion;

            return Assembly.GetExecutingAssembly().GetName().Version;
        }
    }
}
