using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

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
                        onCanExecute: x => File.Exists(this.UserSettings.ClientExecutablePath) && !string.IsNullOrWhiteSpace(this.UserSettings.ClientVersion));
                }

                return launchClientCommand;
            }
        }

        public UserSettingsViewModel UserSettings
        {
            get { return userSettingsViewModel; }
            set
            {
                SetProperty(ref userSettingsViewModel, value);
            }
        }
        #endregion

        public MainViewModel(UserSettings userSettings)
            : base(App.ApplicationName)
        {
            if (userSettings == null)
                throw new ArgumentNullException("userSettings");

            this.userSettingsViewModel = new UserSettingsViewModel(userSettings);
        }

        #region Execute Methods
        void OnLocateClientPath()
        {
            Debug.WriteLine("OnLocateClientPath");
        }

        void OnTestConnection()
        {
            Debug.WriteLine("OnTestConnection");
        }

        void OnLaunchClient()
        {
            Debug.WriteLine("OnLaunchClient");

            Debug.WriteLine("ClientExecutablePath = {0}, ClientVersion = {1}, ServerHostname = {2}, ServerPort = {3}, ShouldRedirectClient = {4}, ShouldSkipIntro = {5}, ShouldAllowMultipleInstances = {6}, ShouldHideWalls = {7}",
                this.UserSettings.ClientExecutablePath, 
                this.UserSettings.ClientVersion, 
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
