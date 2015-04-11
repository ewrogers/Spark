using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using Spark.Input;

namespace Spark.ViewModels
{
    public sealed class MainViewModel : WorkspaceViewModel
    {
        string clientExecutablePath;
        string clientVersion;
        string serverHostname;
        string serverPort;
        bool shouldRedirectClient;
        bool shouldSkipIntro;
        bool shouldAllowMultipleInstances;
        bool shouldHideWalls;

        ICommand locateClientPathCommand;
        ICommand testConnectionCommand;
        ICommand launchClientCommand;

        #region Model Properties
        public string ClientExecutablePath
        {
            get { return clientExecutablePath; }
            set { SetProperty(ref clientExecutablePath, value); }
        }

        public string ClientVersion
        {
            get { return clientVersion; }
            set { SetProperty(ref clientVersion, value); }
        }

        public string ServerHostname
        {
            get { return serverHostname; }
            set { SetProperty(ref serverHostname, value); }
        }

        public string ServerPort
        {
            get { return serverPort; }
            set { SetProperty(ref serverPort, value); }
        }

        public bool ShouldRedirectClient
        {
            get { return shouldRedirectClient; }
            set { SetProperty(ref shouldRedirectClient, value); }
        }

        public bool ShouldSkipIntro
        {
            get { return shouldSkipIntro; }
            set { SetProperty(ref shouldSkipIntro, value); }
        }

        public bool ShouldAllowMultipleInstances
        {
            get { return shouldAllowMultipleInstances; }
            set { SetProperty(ref shouldAllowMultipleInstances, value); }
        }

        public bool ShouldHideWalls
        {
            get { return shouldHideWalls; }
            set { SetProperty(ref shouldHideWalls, value); }
        }
        #endregion

        #region Command Properties
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
                        onCanExecute: x => !string.IsNullOrWhiteSpace(this.ServerHostname) && !string.IsNullOrWhiteSpace(this.ServerPort));

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
                        onCanExecute: x => File.Exists(this.ClientExecutablePath) && !string.IsNullOrWhiteSpace(this.ClientVersion));
                }

                return launchClientCommand;
            }
        }
        #endregion

        public MainViewModel(string displayName)
            : base(displayName) { }

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
                this.ClientExecutablePath, this.ClientVersion, this.ServerHostname, this.ServerPort, this.ShouldRedirectClient, this.ShouldSkipIntro, this.ShouldAllowMultipleInstances, this.ShouldHideWalls);
        }
        #endregion
    }
}
