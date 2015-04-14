using System;
using System.Collections.Generic;
using System.Text;

using Spark.Models;

namespace Spark.ViewModels
{
    public sealed class UserSettingsViewModel : ViewModelBase
    {
        UserSettings userSettings;

        #region Model Properties
        public string ClientExecutablePath
        {
            get { return userSettings.ClientExecutablePath; }
            set
            {
                OnPropertyChanging();
                userSettings.ClientExecutablePath = value;
                OnPropertyChanged();
            }
        }

        public string ClientVersion
        {
            get { return userSettings.ClientVersion; }
            set
            {
                OnPropertyChanging();
                userSettings.ClientVersion = value;
                OnPropertyChanged();
            }
        }

        public bool ShouldAutoDetectClientVersion
        {
            get { return userSettings.ShouldAutoDetectClientVersion; }
            set
            {
                OnPropertyChanging();
                userSettings.ShouldAutoDetectClientVersion = value;
                OnPropertyChanged();
            }
        }

        public string ServerHostname
        {
            get { return userSettings.ServerHostname; }
            set
            {
                OnPropertyChanging();
                userSettings.ServerHostname = value;
                OnPropertyChanged();
            }
        }

        public int ServerPort
        {
            get { return userSettings.ServerPort; }
            set
            {
                OnPropertyChanging();
                userSettings.ServerPort = value;
                OnPropertyChanged();
            }
        }

        public bool ShouldRedirectClient
        {
            get { return userSettings.ShouldRedirectClient; }
            set
            {
                OnPropertyChanging();
                userSettings.ShouldRedirectClient = value;
                OnPropertyChanged();
            }
        }

        public bool ShouldSkipIntro
        {
            get { return userSettings.ShouldSkipIntro; }
            set
            {
                OnPropertyChanging();
                userSettings.ShouldSkipIntro = value;
                OnPropertyChanged();
            }
        }

        public bool ShouldAllowMultipleInstances
        {
            get { return userSettings.ShouldAllowMultipleInstances; }
            set
            {
                OnPropertyChanging();
                userSettings.ShouldAllowMultipleInstances = value;
                OnPropertyChanged();
            }
        }

        public bool ShouldHideWalls
        {
            get { return userSettings.ShouldHideWalls; }
            set
            {
                OnPropertyChanging();
                userSettings.ShouldHideWalls = value;
                OnPropertyChanged();
            }
        }
        #endregion

        public UserSettingsViewModel(UserSettings userSettings)
            : base(null, null)
        {
            if (userSettings == null)
                throw new ArgumentNullException("userSettings");

            this.userSettings = userSettings;
        }
    }
}
