using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

using Spark.Dialogs;
using Spark.Input;

namespace Spark.ViewModels
{
    public enum DialogButtons
    {
        OK = 0,
        OKCancel = 1,
        YesNo = 2
    }

    public class DialogViewModel : WorkspaceViewModel
    {
        string title;
        string message;
        string messageHint;
        string yesButtonTitle;
        string noButtonTitle;
        bool isYesButtonVisible;
        bool isNoButtonVisible;

        ICommand yesCommand;
        ICommand noCommand;

        public event EventHandler YesClicked;
        public event EventHandler NoClicked;

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

        public virtual string YesButtonTitle
        {
            get { return yesButtonTitle; }
            set { SetProperty(ref yesButtonTitle, value); }
        }

        public virtual string NoButtonTitle
        {
            get { return noButtonTitle; }
            set { SetProperty(ref noButtonTitle, value); }
        }

        public virtual bool IsYesButtonVisible
        {
            get { return isYesButtonVisible; }
            set { SetProperty(ref isYesButtonVisible, value); }
        }

        public virtual bool IsNoButtonVisible
        {
            get { return isNoButtonVisible; }
            set { SetProperty(ref isNoButtonVisible, value); }
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
        #endregion

        public DialogViewModel(string title, string message, string messageHint = null, DialogButtons buttons = DialogButtons.OK, IDialogService dialogService = null)
            : base(title, dialogService)
        {
            this.Title = title;
            this.Message = message;
            this.MessageHint = messageHint;

            if (buttons == DialogButtons.OK || buttons == DialogButtons.OKCancel)
            {
                this.YesButtonTitle = "_OK";
                this.NoButtonTitle = "_Cancel";
                this.IsYesButtonVisible = true;
                this.IsNoButtonVisible = (buttons == DialogButtons.OKCancel);
            }
            else if (buttons == DialogButtons.YesNo)
            {
                this.YesButtonTitle = "_Yes";
                this.NoButtonTitle = "_No";
                this.IsYesButtonVisible = true;
                this.IsNoButtonVisible = true;
            }
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
    }
}
