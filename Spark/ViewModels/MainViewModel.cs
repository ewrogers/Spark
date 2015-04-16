using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using Microsoft.Win32;

using Spark.Dialogs;
using Spark.Input;
using Spark.Interop;
using Spark.Models;

namespace Spark.ViewModels
{
    public sealed class MainViewModel : WorkspaceViewModel
    {
        ICommand locateClientPathCommand;
        ICommand testConnectionCommand;
        ICommand launchClientCommand;

        UserSettings userSettings;
        IEnumerable<ClientVersion> clientVersions;

        UserSettingsViewModel userSettingsViewModel;
        ObservableCollection<ClientVersionViewModel> clientVersionViewModels;

        #region Properties
        public ICommand LocateClientPathCommand
        {
            get
            {
                if (locateClientPathCommand == null)
                    locateClientPathCommand = new DelegateCommand(parameter => OnLocateClientPath());

                return locateClientPathCommand;
            }
        }

        public ICommand TestConnectionCommand
        {
            get
            {
                // Lazy-initialized
                if (testConnectionCommand == null)
                    testConnectionCommand = new DelegateCommand(x => OnTestConnection(),
                        onCanExecute: x => !string.IsNullOrWhiteSpace(this.UserSettings.ServerHostname) && this.UserSettings.ServerPort > 0);

                return testConnectionCommand;
            }
        }

        public ICommand LaunchClientCommand
        {
            get
            {
                // Lazy-initialized
                if (launchClientCommand == null)
                {
                    launchClientCommand = new DelegateCommand(x => OnLaunchClient(),
                        onCanExecute: x => File.Exists(this.UserSettings.ClientExecutablePath) && (!string.IsNullOrWhiteSpace(this.UserSettings.ClientVersion) || this.UserSettings.ShouldAutoDetectClientVersion));
                }

                return launchClientCommand;
            }
        }

        public UserSettingsViewModel UserSettings
        {
            get { return userSettingsViewModel; }
            set { SetProperty(ref userSettingsViewModel, value); }
        }

        public ObservableCollection<ClientVersionViewModel> ClientVersions
        {
            get { return clientVersionViewModels; }
            set { SetProperty(ref clientVersionViewModels, value); }
        }
        #endregion

        public MainViewModel(UserSettings userSettings, IEnumerable<ClientVersion> clientVersions, IDialogService dialogService)
            : base(App.ApplicationName, dialogService)
        {
            if (userSettings == null)
                throw new ArgumentNullException("userSettings");

            if (clientVersions == null)
                throw new ArgumentNullException("clientVersions");

            if (dialogService == null)
                throw new ArgumentNullException("dialogService");

            this.userSettings = userSettings;
            this.clientVersions = clientVersions;

            // Create user settings view model
            this.userSettingsViewModel = new UserSettingsViewModel(userSettings);

            // Create client version view model for each client settings object (via LINQ projection)
            var clientVersionViewModels = clientVersions.Select(x => new ClientVersionViewModel(x));
            this.clientVersionViewModels = new ObservableCollection<ClientVersionViewModel>(clientVersionViewModels);
        }

        #region Execute Methods
        void OnLocateClientPath()
        {
            Debug.WriteLine("OnLocateClientPath");

            // Create open file dialog to show user
            var dialog = new OpenFileDialog()
            {
                FileName = "Darkages.exe",
                DefaultExt = ".exe",
                Filter = "Dark Ages Game Clients|Darkages.exe|All Executables (*.exe)|*.exe"
            };

            // Show dialog to user
            if (dialog.ShowDialog() == true)
            {
                // Set selected filename
                this.UserSettings.ClientExecutablePath = dialog.FileName;
            }
        }

        void OnTestConnection()
        {
            Debug.WriteLine("OnTestConnection");

            Debug.WriteLine("ServerHostname = {0},  ServerPort = {1}", this.UserSettings.ServerHostname, this.UserSettings.ServerPort);

            var result = this.DialogService.ShowOKDialog("Not Implemented", "Sorry, this feature is not currently implemented.", "It will be implemented in a future update.");
            Debug.WriteLine("Result = {0}", result);
        }

        void OnLaunchClient()
        {
            Debug.WriteLine("OnLaunchClient");

            Debug.WriteLine("ClientExecutablePath = {0}, ClientVersion = {1}, Auto-Detect = {2}, ServerHostname = {3}, ServerPort = {4}, ShouldRedirectClient = {5}, ShouldSkipIntro = {6}, ShouldAllowMultipleInstances = {7}, ShouldHideWalls = {8}",
                this.UserSettings.ClientExecutablePath,
                this.UserSettings.ClientVersion,
                this.UserSettings.ShouldAutoDetectClientVersion,
                this.UserSettings.ServerHostname,
                this.UserSettings.ServerPort,
                this.UserSettings.ShouldRedirectClient,
                this.UserSettings.ShouldSkipIntro,
                this.UserSettings.ShouldAllowMultipleInstances,
                this.UserSettings.ShouldHideWalls);

            LaunchClientWithSettings(userSettings, clientVersions);
        }
        #endregion

        #region Client Launch Methods
        void LaunchClientWithSettings(UserSettings settings, IEnumerable<ClientVersion> clientVersions)
        {
            if (settings == null)
                throw new ArgumentNullException("settings");

            if (clientVersions == null)
                throw new ArgumentNullException("clientVersions");

            ClientVersion clientVersion = null;

            #region Verify Client Executable
            if (!File.Exists(settings.ClientExecutablePath))
            {
                Debug.WriteLine("ClientExecutableNotFound: {0}", settings.ClientExecutablePath);

                this.DialogService.ShowOKDialog("File Not Found",
                    "Unable to locate the client executable.",
                    string.Format("Ensure the file exists at the following location:\n{0}", settings.ClientExecutablePath));

                return;
            }
            #endregion

            #region Determine Client Version
            if (settings.ShouldAutoDetectClientVersion)
            {
                // Auto-detect using the MD5
                var md5HashString = "ca31b8165ea7409d285d81616d8ca4f2"; // 7.39
                clientVersion = clientVersions.FirstOrDefault(x => x.Hash.Equals(md5HashString, StringComparison.OrdinalIgnoreCase));
            }
            else
            {
                // Manually select client by name
                clientVersion = clientVersions.FirstOrDefault(x => x.Name.Equals(settings.ClientVersion, StringComparison.OrdinalIgnoreCase));
            }

            // Check that a client version was determined
            if (clientVersion == null)
            {
                Debug.WriteLine("ClientVersionNotFound: Auto-Detect={0}, VersionName={1}", settings.ShouldAutoDetectClientVersion, settings.ClientVersion);

                this.DialogService.ShowOKDialog("Unknown Client Version",
                    "Unable to determine the client version.",
                    "You may manually select a client version by disabling auto-detection.");

                return;
            }
            #endregion

            #region Launch and Patch Client
            try
            {
                // Try to launch the client
                using (var process = SuspendedProcess.Start(settings.ClientExecutablePath))
                {
                    try
                    {
                        PatchClient(settings, process, clientVersion);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(string.Format("UnableToPachClient: {0}", ex.Message));

                        this.DialogService.ShowOKDialog("Failed to Patch",
                            "Unable to patch the client executable.",
                            ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(string.Format("UnableToLaunchClient: {0}", ex.Message));

                this.DialogService.ShowOKDialog("Failed to Launch",
                    "Unable to launch the client executable.",
                    ex.Message);
            }
            #endregion
        }

        void PatchClient(UserSettings settings, SuspendedProcess process, ClientVersion clientVersion)
        {
            if (settings == null)
                throw new ArgumentNullException("settings");

            if (process == null)
                throw new ArgumentNullException("process");

            if (clientVersion == null)
                throw new ArgumentNullException("clientVersion");

            // TODO: patch client here
        }
        #endregion
    }
}
