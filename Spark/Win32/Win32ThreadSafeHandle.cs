using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Win32.SafeHandles;

namespace Spark.Win32
{
    internal sealed class Win32ThreadSafeHandle : SafeHandleZeroOrMinusOneIsInvalid
    {
        public Win32ThreadSafeHandle()
            : base(true) { }

        #region SafeHandle Methods
        protected override bool ReleaseHandle()
        {
            return NativeMethods.CloseHandle(handle);
        }
        #endregion
    }
}
