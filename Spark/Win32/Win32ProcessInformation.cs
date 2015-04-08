using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Spark.Win32
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct Win32ProcessInformation
    {
        public Win32ProcessSafeHandle ProcessHandle { get; set; }
        public Win32ThreadSafeHandle ThreadHandle { get; set; }

        public int ProcessId { get; set; }
        public int ThreadId { get; set; }
    }
}
