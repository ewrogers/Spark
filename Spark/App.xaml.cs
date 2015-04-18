using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Deployment.Application;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using System.Windows;

using Spark.Dialogs;
using Spark.Models;
using Spark.Models.Serializers;
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
            var dialogService = new DialogService(window);
            var viewModel = new MainViewModel(this.CurrentSettings, this.ClientVersions, dialogService);

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
        static void SaveUserSettings(string fileName, UserSettings settings)
        {
            if (fileName == null)
                throw new ArgumentNullException("fileName");

            if (settings == null)
                throw new ArgumentNullException("settings");

            try
            {
                var xml = new XDocument(
                    new XDeclaration("1.0", "utf-8", "yes"),
                    settings.Serialize());

                // Save user settings to file
                xml.Save(fileName);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(string.Format("Unable to save user settings: {0}", ex.Message));
            }
        }

        static UserSettings LoadSettingsOrDefaults(string fileName)
        {
            if (fileName == null)
                throw new ArgumentNullException("fileName");

            try
            {
                // Load user settings from file
                if (File.Exists(fileName))
                {
                    var xml = XDocument.Load(fileName);
                    return UserSettingsSerializer.DeserializeAll(xml).FirstOrDefault() ?? UserSettings.CreateDefaults();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(string.Format("Unable to load user settings: {0}", ex.Message));
            }

            return UserSettings.CreateDefaults();
        }
        #endregion

        #region Save/Load Client Versions
        static void SaveClientVersions(string fileName, IEnumerable<ClientVersion> versions)
        {
            if (fileName == null)
                throw new ArgumentNullException("fileName");

            if (versions == null)
                throw new ArgumentNullException("versions");

            try
            {
                // Save client versions to file
                var xml = new XDocument(
                    new XDeclaration("1.0", "utf-8", "yes"),
                    new XElement("versions", versions.SerializeAll()));

                xml.Save(fileName);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(string.Format("Unable to save client versions: {0}", ex.Message));
            }
        }

        static IEnumerable<ClientVersion> LoadClientVersionsOrDefaults(string fileName)
        {
            if (fileName == null)
                throw new ArgumentNullException("fileName");

            IEnumerable<ClientVersion> defaults = new[] { ClientVersion.Version739, ClientVersion.Version737 };
            IEnumerable<ClientVersion> userVersions = null;

            try
            {
                // Load client versions from file
                if (File.Exists(fileName))
                {
                    var xml = XDocument.Load(fileName);
                    userVersions = ClientVersionSerializer.DeserializeAll(xml);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(string.Format("Unable to load client versions: {0}", ex.Message));
            }

            if (userVersions != null)
            {
                userVersions = userVersions.Union(defaults, new ClientVersion.VersionComparer());
            }
            else
            {
                userVersions = defaults;
            }

            return userVersions;
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
