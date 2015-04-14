using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

using Spark.Views;

namespace Spark.Dialogs
{
    public class DialogService : IDialogService
    {
        #region Properties
        public Window Owner { get; protected set; }
        #endregion

        public DialogService()
            : this(null) { }

        public DialogService(Window owner)
        {
            this.Owner = owner;
        }

        #region IDialogService Methods
        public virtual bool? ShowDialog(object dataContext)
        {
            var dialog = new DialogWindow();
            dialog.Owner = this.Owner;

            dialog.DataContext = dataContext;
            return dialog.ShowDialog();
        }
        #endregion
    }
}
