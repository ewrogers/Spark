using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using Spark.Input;

namespace Spark.ViewModels
{
    public sealed class MainViewModel : WorkspaceViewModel
    {
        ICommand locateClientPathCommand;
        ICommand testConnectionCommand;
        ICommand launchClientCommand;

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
                    testConnectionCommand = new DelegateCommand(parameter => OnTestConnection());

                return testConnectionCommand;
            }
        }

        public ICommand LaunchClientCommand
        {
            get
            {
                // Lazy-initialized
                if (launchClientCommand == null)
                    launchClientCommand = new DelegateCommand(parameter => OnLaunchClient());

                return launchClientCommand;
            }
        }
        #endregion

        public MainViewModel(string displayName)
            : base(displayName) { }

        #region Command Methods
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
        }
        #endregion
    }
}
