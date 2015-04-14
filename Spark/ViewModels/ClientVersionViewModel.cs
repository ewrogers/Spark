using System;
using System.Collections.Generic;
using System.Text;

using Spark.Models;

namespace Spark.ViewModels
{
    public sealed class ClientVersionViewModel : ViewModelBase
    {
        ClientVersion clientVersion;

        #region Model Properties
        public string Name
        {
            get { return clientVersion.Name; }
            set
            {
                OnPropertyChanging();
                clientVersion.Name = value;
                OnPropertyChanged();
            }
        }

        public int VersionCode
        {
            get { return clientVersion.VersionCode; }
            set
            {
                OnPropertyChanging();
                clientVersion.VersionCode = value;
                OnPropertyChanged();
            }
        }

        public string Hash
        {
            get { return clientVersion.Hash; }
            set
            {
                OnPropertyChanging();
                clientVersion.Hash = value;
                OnPropertyChanged();
            }
        }

        public long ServerAddressPatchAddress
        {
            get { return clientVersion.ServerAddressPatchAddress; }
            set
            {
                OnPropertyChanged();
                clientVersion.ServerAddressPatchAddress = value;
                OnPropertyChanging();
            }
        }

        public long ServerPortPatchAddress
        {
            get { return clientVersion.ServerPortPatchAddress; }
            set
            {
                OnPropertyChanging();
                clientVersion.ServerPortPatchAddress = value;
                OnPropertyChanged();
            }
        }

        public long IntroVideoPatchAddress
        {
            get { return clientVersion.IntroVideoPatchAddress; }
            set
            {
                OnPropertyChanging();
                clientVersion.IntroVideoPatchAddress = value;
                OnPropertyChanged();
            }
        }

        public long MultipleInstancePatchAddress
        {
            get { return clientVersion.MultipleInstancePatchAddress; }
            set
            {
                OnPropertyChanging();
                clientVersion.MultipleInstancePatchAddress = value;
                OnPropertyChanged();
            }
        }

        public long HideWallsPatchAddress
        {
            get { return clientVersion.HideWallsPatchAddress; }
            set
            {
                OnPropertyChanging();
                clientVersion.HideWallsPatchAddress = value;
                OnPropertyChanged();
            }
        }
        #endregion

        public ClientVersionViewModel(ClientVersion clientVersion)
            : base(null, null)
        {
            if (clientVersion == null)
                throw new ArgumentNullException("clientVersion");

            this.clientVersion = clientVersion;
        }
    }
}
