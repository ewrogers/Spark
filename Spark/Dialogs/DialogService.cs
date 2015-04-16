using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

using Spark.ViewModels;
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
        public virtual bool? ShowDialog<T>(T dataContext) where T : DialogViewModel
        {
            var dialog = new DialogWindow();
            dialog.Owner = this.Owner;

            dataContext.RequestClose += delegate { dialog.Close(); };

            dataContext.NegativeButtonClicked += delegate
            {
                dialog.DialogResult = false;
                dialog.Close();
            };

            dataContext.PositiveButtonClicked += delegate
            {
                dialog.DialogResult = true;
                dialog.Close();
            };

            dialog.DataContext = dataContext;
            return dialog.ShowDialog();
        }
        #endregion
    }
}
