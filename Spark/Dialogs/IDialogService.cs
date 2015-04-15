using System;
using System.Collections.Generic;
using System.Text;

using Spark.ViewModels;

namespace Spark.Dialogs
{
    public interface IDialogService
    {
        bool? ShowDialog<T>(T dataContext) where T : DialogViewModel;
    }
}
