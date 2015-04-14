using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Deployment.Application;
using System.Diagnostics;
using System.IO;
using System.Linq;
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
        public static readonly string ClientVersionsFileName = "Versions.xml";

        #region Properties
        public UserSettings CurrentSettings { get; protected set; }
        public IEnumerable<ClientVersion> ClientVersions { get; protected set; }
        #endregion

        #region Application Lifecycle
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Load settings and client versions from file (or defaults)
            this.CurrentSettings = LoadSettingsOrDefaults(App.SettingsFileName);
            this.ClientVersions = LoadClientVersionsOrDefaults(App.ClientVersionsFileName);

            // Initialize the main window and view model
            var window = new MainWindow();
            var viewModel = new MainViewModel(this.CurrentSettings, this.ClientVersions);

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
            SaveUserSettings(App.SettingsFileName, this.CurrentSettings);
            SaveClientVersions(App.ClientVersionsFileName, this.ClientVersions);

            base.OnExit(e);
        }
        #endregion

        #region Save/Load User Settings
        static void SaveUserSettings(string filename, UserSettings settings)
        {
            if (filename == null)
                throw new ArgumentNullException("filename");

            if (settings == null)
                throw new ArgumentNullException("settings");

            try
            {
                // Save user settings to file
                settings.SaveToFile(SettingsFileName);
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
                if (File.Exists(filename))
                    return UserSettings.LoadFromFile(filename);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Unable to load user settings: {0}", ex.Message);
            }

            return UserSettings.CreateDefaults();
        }
        #endregion

        #region Save/Load Client Versions
        static void SaveClientVersions(string filename, IEnumerable<ClientVersion> versions)
        {
            if (filename == null)
                throw new ArgumentNullException("filename");

            if (versions == null)
                throw new ArgumentNullException("versions");

            try
            {
                // Save client versions to file
                ClientVersion.SaveToFile(filename, versions);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Unable to save client versions: {0}", ex.Message);
            }
        }

        static IEnumerable<ClientVersion> LoadClientVersionsOrDefaults(string filename)
        {
            try
            {
                // Load client versions from file
                if (File.Exists(filename))
                    return ClientVersion.LoadFromFile(filename).OrderBy(x => x.Name);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Unable to load client versions: {0}", ex.Message);
            }

            return new[] { ClientVersion.Version739 };
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
