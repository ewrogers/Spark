using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Spark.Win32
{
    internal static class NativeMethods
    {
        [DllImport("kernel32", SetLastError = true)]
        public static extern Win32ProcessSafeHandle OpenProcess(int processId, bool inheritHandle, Win32ProcessAccess desiredAccess);
        
        [DllImport("kernel32", SetLastError = true)]
        public static extern bool ReadProcessMemory(Win32ProcessSafeHandle processHandle, IntPtr baseAddress, byte[] buffer, int count, out int numberOfBytesRead);

        [DllImport("kernel32", SetLastError = true)]
        public static extern bool WriteProcessMemory(Win32ProcessSafeHandle processHandle, IntPtr baseAddress, byte[] buffer, int count, out int numberOfBytesWritten);

        [DllImport("kernel32", SetLastError = true)]
        public static extern bool CloseHandle(IntPtr handle);
    }
}
