using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spark.ViewModels
{
    public sealed class MainWindowViewModel : WorkspaceViewModel
    {
        public MainWindowViewModel(string displayName)
            : base(displayName) { }
    }
}
