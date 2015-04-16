using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Spark.Win32
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct Win32ProcessInformation
    {
        public IntPtr ProcessHandle { get; set; }
        public IntPtr ThreadHandle { get; set; }

        public int ProcessId { get; set; }
        public int ThreadId { get; set; }
    }
}
