using System;
using System.Collections.Generic;
using System.Text;

namespace Spark.Dialogs
{
    public interface IDialogService
    {
        bool? ShowDialog(object dataContext);
    }
}
