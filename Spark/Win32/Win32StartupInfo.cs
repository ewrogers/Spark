using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Spark.Win32
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct Win32StartupInfo
    {
        public int Size { get; set; }
        public string ReservedString { get; set; }
        public string Desktop { get; set; }
        public string Title { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int ConsoleWidth { get; set; }
        public int ConsoleHeight { get; set; }
        public int FillAttribute { get; set; }
        public int Flags { get; set; }
        public short ShowWindow { get; set; }
        public short ReservedInt16 { get; set; }
        public IntPtr ReservedBytes { get; set; }
        public IntPtr StandardInput { get; set; }
        public IntPtr StandardOutput { get; set; }
        public IntPtr StandardError { get; set; }
    }
}
