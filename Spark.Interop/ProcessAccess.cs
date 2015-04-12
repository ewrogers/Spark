using System;
using System.Collections.Generic;
using System.Text;

namespace Spark.Interop
{
    [Flags]
    public enum ProcessAccess : int
    {
        Read = 0x1,
        Write = 0x2,
        ReadWrite = Read | Write
    }
}
