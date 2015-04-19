using System;

namespace Spark.Common
{
    public static class IntegerExtender
    {
        static readonly uint ByteMask = 0xFF;
        static readonly uint WordMask = 0xFFFF;
        static readonly uint DoubleWordMask = 0xFFFFFFFF;

        #region Get LoByte Methods
        public static byte LoByte(this short value) { return (byte)(value & ByteMask); }
        public static byte LoByte(this ushort value) { return (byte)(value & ByteMask); }
        #endregion

        #region Get HiByte Methods
        public static byte HiByte(this short value) { return (byte)((value >> 8) & ByteMask); }
        public static byte HiByte(this ushort value) { return (byte)((value >> 8) & ByteMask); }
        #endregion

        #region Get LoWord Methods
        public static short LoWord(this int value) { return (short)(value & WordMask); }
        public static ushort LoWord(this uint value) { return (ushort)(value & WordMask); }
        #endregion

        #region Get HiWord Methods
        public static short HiWord(this int value) { return (short)((value >> 16) & WordMask); }
        public static ushort HiWord(this uint value) { return (ushort)((value >> 16) & WordMask); }
        #endregion

        #region Get LoDoubleWord Methods
        public static int LoDoubleWord(this long value) { return (int)(value & DoubleWordMask); }
        public static uint LoDoubleWord(this ulong value) { return (uint)(value & DoubleWordMask); }
        #endregion

        #region Get HiDoubleWord Methods
        public static int HiDoubleWord(this long value) { return (int)((value >> 32) & DoubleWordMask); }
        public static uint HiDoubleWord(this ulong value) { return (uint)((value >> 32) & DoubleWordMask); }
        #endregion

        #region Make Word Methods
        public static short MakeWord(byte loByte, byte hiByte) { return (short)((hiByte << 8) | loByte); }
        public static ushort MakeWordUnsigned(byte loByte, byte hiByte) { return (ushort)((hiByte << 8) | loByte); }
        #endregion

        #region Make DoubleWord Methods
        public static int MakeDoubleWord(short loWord, short hiWord) { return ((ushort)hiWord << 16) | (ushort)loWord; }
        public static uint MakeDoubleWordUnsigned(ushort loWord, ushort hiWord) { return (uint)((hiWord << 16) | loWord); }
        #endregion

        #region Make QuadWord Methods
        public static long MakeQuadWord(int loDoubleWord, int hiDoubleWord) { return (long)MakeQuadWordUnsigned((uint)loDoubleWord, (uint)hiDoubleWord); }

        public static ulong MakeQuadWordUnsigned(uint loDoubleWord, uint hiDoubleWord)
        {
            var loLong = (ulong)loDoubleWord;
            var hiLong = (ulong)hiDoubleWord;
            
            return ((hiLong << 32) | loLong);
        }
        #endregion
    }
}
