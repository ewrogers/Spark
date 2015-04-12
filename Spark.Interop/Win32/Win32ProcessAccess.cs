using System;
using System.Collections.Generic;
using System.Text;

namespace Spark.Win32
{
    [Flags]
    internal enum Win32ProcessAccess : uint
    {
        None = 0x0,
        Terminate = 0x1,
        CreateThread = 0x2,
        VmOperation = 0x8,
        VmRead = 0x10,
        VmWrite = 0x20,
        QueryInformation = 0x400
    }
}
