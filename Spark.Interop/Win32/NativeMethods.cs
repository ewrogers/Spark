using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Spark.Win32
{
    internal static class NativeMethods
    {
        [DllImport("kernel32", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool CreateProcess(string applicationPath,
            string commandLine,
            IntPtr processSecurityAttributes,
            IntPtr threadSecurityAttributes,
            bool inheritHandles,
            Win32ProcessCreationFlags creationFlags,
            IntPtr environment,
            string currentDirectory,
            ref Win32StartupInfo startupInfo,
            out Win32ProcessInformation processInformation);

        [DllImport("kernel32", SetLastError = true)]
        public static extern Win32ProcessSafeHandle OpenProcess(Win32ProcessAccess desiredAccess, 
            bool inheritHandle, 
            int processId);
        
        [DllImport("kernel32", SetLastError = true)]
        public static extern bool ReadProcessMemory(Win32ProcessSafeHandle processHandle, 
            IntPtr baseAddress, 
            byte[] buffer, 
            int count, 
            out int numberOfBytesRead);

        [DllImport("kernel32", SetLastError = true)]
        public static extern bool WriteProcessMemory(Win32ProcessSafeHandle processHandle, 
            IntPtr baseAddress, 
            byte[] buffer, 
            int count, 
            out int numberOfBytesWritten);

        [DllImport("kernel32", SetLastError = true)]
        public static extern int SuspendThread(Win32ThreadSafeHandle threadHandle);

        [DllImport("kernel32", SetLastError = true)]
        public static extern int ResumeThread(Win32ThreadSafeHandle threadHandle);

        [DllImport("kernel32", SetLastError = true)]
        public static extern bool CloseHandle(IntPtr handle);

        [DllImport("kernel32")]
        public static extern int GetLastError();
    }
}
