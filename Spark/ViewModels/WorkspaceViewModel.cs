using System;
using System.Windows.Input;

using Spark.Dialogs;
using Spark.Input;

namespace Spark.ViewModels
{
    public abstract class WorkspaceViewModel : ViewModelBase
    {
        ICommand closeCommand;

        public event EventHandler RequestClose;

        #region Properties
        public ICommand CloseCommand
        {
            get
            {
                // Lazy initialized
                if (closeCommand == null)
                    closeCommand = new DelegateCommand(parameter => OnRequestClose());

                return closeCommand;
            }
        }
        #endregion

        protected WorkspaceViewModel(string displayName = null, IDialogService dialogService = null)
            : base(displayName, dialogService) { }

        protected virtual void OnRequestClose()
        {
            var handler = this.RequestClose;

            if (handler != null)
                handler(this, EventArgs.Empty);
        }
    }
}
