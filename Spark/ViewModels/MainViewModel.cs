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
using Spark.Models;

namespace Spark.ViewModels
{
    public sealed class MainViewModel : WorkspaceViewModel
    {
        ICommand locateClientPathCommand;
        ICommand testConnectionCommand;
        ICommand launchClientCommand;

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

            var dataContext = new DialogViewModel("Error Message", "Something bad has occured!", "Sorry, this really should not happen.");
            var result = this.DialogService.ShowDialog(dataContext);

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
        }
        #endregion
    }
}
