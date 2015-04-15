using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

using Spark.Dialogs;
using Spark.Input;

namespace Spark.ViewModels
{
    public class DialogViewModel : WorkspaceViewModel
    {
        string title;
        string message;
        string messageHint;

        ICommand yesCommand;
        ICommand noCommand;
        ICommand okCommand;
        ICommand cancelCommand;

        public event EventHandler YesClicked;
        public event EventHandler NoClicked;
        public event EventHandler OKClicked;
        public event EventHandler CancelClicked;

        #region Properties
        public virtual string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        public virtual string Message
        {
            get { return message; }
            set { SetProperty(ref message, value); }
        }

        public virtual string MessageHint
        {
            get { return messageHint; }
            set { SetProperty(ref messageHint, value); }
        }

        public ICommand YesCommand
        {
            get
            {
                // Lazy-initialized
                if (yesCommand == null)
                    yesCommand = new DelegateCommand(x => OnYesClicked());

                return yesCommand;
            }
        }

        public ICommand NoCommand
        {
            get
            {
                // Lazy-initialized
                if (noCommand == null)
                    noCommand = new DelegateCommand(x => OnNoClicked());

                return noCommand;
            }
        }

        public ICommand OKCommand
        {
            get
            {
                // Lazy-initialized
                if (okCommand == null)
                    okCommand = new DelegateCommand(x => OnOKClicked());

                return okCommand;
            }
        }

        public ICommand CancelCommand
        {
            get
            {
                // Lazy-initialized
                if (cancelCommand == null)
                    cancelCommand = new DelegateCommand(x => OnCancelClicked());

                return cancelCommand;
            }
        }
        #endregion

        public DialogViewModel(string title, string message, string messageHint = null, IDialogService dialogService = null)
            : base(title, dialogService)
        {
            this.Title = title;
            this.Message = message;
            this.MessageHint = messageHint;
        }

        protected virtual void OnYesClicked()
        {
            var handler = this.YesClicked;

            if (handler != null)
                handler(this, EventArgs.Empty);
        }

        protected virtual void OnNoClicked()
        {
            var handler = this.NoClicked;

            if (handler != null)
                handler(this, EventArgs.Empty);
        }

        protected virtual void OnOKClicked()
        {
            var handler = this.OKClicked;

            if (handler != null)
                handler(this, EventArgs.Empty);
        }

        protected virtual void OnCancelClicked()
        {
            var handler = this.CancelClicked;

            if (handler != null)
                handler(this, EventArgs.Empty);
        }
    }
}
