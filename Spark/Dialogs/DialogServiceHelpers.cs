using System;
using System.Collections.Generic;
using System.Text;

using Spark.ViewModels;

namespace Spark.Dialogs
{
    public static class DialogServiceHelpers
    {
        #region Helper Methods
        public static bool? ShowOKDialog(this IDialogService dialogService, string title, string message, string messageHint = null, string okButtonTitle = null)
        {
            var context = new DialogViewModel(title, message, messageHint, DialogButtons.OK, dialogService);
            context.YesButtonTitle = okButtonTitle ?? "_OK";

            return dialogService.ShowDialog(context);
        }

        public static bool? ShowOKCancelDialog(this IDialogService dialogService, string title, string message, string messageHint = null, string okButtonTitle = null, string cancelButtonTitle = null)
        {
            var context = new DialogViewModel(title, message, messageHint, DialogButtons.OKCancel, dialogService);
            context.YesButtonTitle = okButtonTitle ?? "_OK";
            context.NoButtonTitle = cancelButtonTitle ?? "_Cancel";

            return dialogService.ShowDialog(context);
        }

        public static bool? ShowYesNoDialog(this IDialogService dialogService, string title, string message, string messageHint = null, string yesButtonTitle = null, string noButtonTitle = null)
        {
            var context = new DialogViewModel(title, message, messageHint, DialogButtons.OKCancel, dialogService);
            context.YesButtonTitle = yesButtonTitle ?? "_Yes";
            context.NoButtonTitle = noButtonTitle ?? "_No";

            return dialogService.ShowDialog(context);
        }
        #endregion
    }
}
