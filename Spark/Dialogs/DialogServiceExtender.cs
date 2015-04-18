using System;

using Spark.ViewModels;

namespace Spark.Dialogs
{
    public static class DialogServiceExtender
    {
        #region Helper Methods
        public static bool? ShowOKDialog(this IDialogService dialogService, string title, string message, string messageHint = null, string okButtonTitle = null)
        {
            if (dialogService == null)
                throw new ArgumentNullException("dialogService");

            var context = new DialogViewModel(title, message, messageHint, DialogButtons.OK, dialogService);
            context.PositiveButtonTitle = okButtonTitle ?? "_OK";

            return dialogService.ShowDialog(context);
        }

        public static bool? ShowOKCancelDialog(this IDialogService dialogService, string title, string message, string messageHint = null, string okButtonTitle = null, string cancelButtonTitle = null)
        {
            if (dialogService == null)
                throw new ArgumentNullException("dialogService");

            var context = new DialogViewModel(title, message, messageHint, DialogButtons.OKCancel, dialogService);
            context.PositiveButtonTitle = okButtonTitle ?? "_OK";
            context.NegativeButtonTitle = cancelButtonTitle ?? "_Cancel";

            return dialogService.ShowDialog(context);
        }

        public static bool? ShowYesNoDialog(this IDialogService dialogService, string title, string message, string messageHint = null, string yesButtonTitle = null, string noButtonTitle = null)
        {
            if (dialogService == null)
                throw new ArgumentNullException("dialogService");

            var context = new DialogViewModel(title, message, messageHint, DialogButtons.YesNo, dialogService);
            context.PositiveButtonTitle = yesButtonTitle ?? "_Yes";
            context.NegativeButtonTitle = noButtonTitle ?? "_No";

            return dialogService.ShowDialog(context);
        }
        #endregion
    }
}
