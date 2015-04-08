using System;
using System.Collections.Generic;
using System.Text;

namespace Spark.Win32
{
    [Flags]
    internal enum Win32ProcessCreationFlags : uint
    {
        None = 0x0,
        DebugProcess = 0x1,
        DebugOnlyThisProcess = 0x2,
        Suspended = 0x4,
        DetachedProcess = 0x8
    }
}
