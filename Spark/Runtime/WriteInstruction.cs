using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Spark.Runtime
{
    public sealed class WriteInstruction : RuntimeInstruction
    {
        #region Properties
        public RuntimeType Type { get; set; }

        public object Value { get; set; }

        public long Address { get; set; }
        #endregion

        public WriteInstruction() { }

        #region RuntimeInstruction Methods
        public override void Execute(Stream stream)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");

            // Seek to address
            stream.Position = this.Address;

            using (var writer = new BinaryWriter(stream, Encoding.UTF8, leaveOpen: true))
            {
                switch(this.Type)
                {
                    case RuntimeType.Char:
                        writer.Write((char)this.Value);
                        break;

                    case RuntimeType.Byte:
                        writer.Write((byte)this.Value);
                        break;
                        
                    case RuntimeType.Boolean:
                        writer.Write((bool)this.Value);
                        break;

                    case RuntimeType.Int16:
                        writer.Write((short)this.Value);
                        break;

                    case RuntimeType.Int32:
                        writer.Write((int)this.Value);
                        break;

                    case RuntimeType.Int64:
                        writer.Write((long)this.Value);
                        break;

                    case RuntimeType.UInt16:
                        writer.Write((ushort)this.Value);
                        break;

                    case RuntimeType.UInt32:
                        writer.Write((uint)this.Value);
                        break;

                    case RuntimeType.UInt64:
                        writer.Write((ulong)this.Value);
                        break;

                    case RuntimeType.ByteArray:
                        writer.Write((byte[])this.Value);
                        break;

                    case RuntimeType.String:
                        writer.Write(this.Value.ToString().ToCharArray());
                        break;

                    default:
                        throw new InvalidOperationException("Invalid runtime type, cannot write value");
                }
            }
        }
        #endregion
    }
}
