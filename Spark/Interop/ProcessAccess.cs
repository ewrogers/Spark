using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
