using System;

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
