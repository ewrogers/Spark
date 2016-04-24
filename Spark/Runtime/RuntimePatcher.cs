using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

using Spark.Models;

namespace Spark.Runtime
{
    public sealed class RuntimePatcher : IDisposable
    {
        bool isDisposed;
        ClientVersion clientVersion;
        Stream stream;
        BinaryWriter writer;

        public RuntimePatcher(ClientVersion clientVersion, Stream stream, bool leaveOpen = false)
        {
            if (clientVersion == null)
                throw new ArgumentNullException("clientVersion");

            if (stream == null)
                throw new ArgumentNullException("stream");

            this.clientVersion = clientVersion;
            this.stream = stream;
            this.writer = new BinaryWriter(stream, Encoding.UTF8, leaveOpen);
        }

        #region Patch Methods
        public void ApplyServerHostnamePatch(IPAddress ipAddress)
        {
            if (ipAddress == null)
                throw new ArgumentNullException("ipAddress");

            ApplyServerHostnamePatch(ipAddress.GetAddressBytes());
        }

        public void ApplyServerHostnamePatch(byte[] ipAddressBytes)
        {
            if (ipAddressBytes == null)
                throw new ArgumentNullException("ipAddressBytes");

            CheckIfDisposed();

            stream.Position = clientVersion.ServerHostnamePatchAddress;

            // Write IP bytes in reverse
            foreach (var ipByte in ipAddressBytes.Reverse())
            {
                writer.Write((byte)0x6A);   // PUSH
                writer.Write((byte)ipByte);
            }

            // No hostname lookup
            if (clientVersion.VersionCode >= 741)
            {
                stream.Position = clientVersion.SkipHostnamePatchAddress;
                for (int i = 0; i < 13; i++)
                {
                    writer.Write((byte)0x90); // NOP
                }
            }
        }

        public void ApplyServerPortPatch(int port)
        {
            if (port <= 0)
                throw new ArgumentOutOfRangeException("Port must be greater than zero");

            CheckIfDisposed();

            stream.Position = clientVersion.ServerPortPatchAddress;

            var portHiByte = (port >> 8) & 0xFF;
            var portLoByte = port & 0xFF;

            // Write lo and hi order bytes
            writer.Write((byte)portLoByte);
            writer.Write((byte)portHiByte);
        }

        public void ApplySkipIntroVideoPatch()
        {
            CheckIfDisposed();

            stream.Position = clientVersion.IntroVideoPatchAddress;

            writer.Write((byte)0x83);   // CMP
            writer.Write((byte)0xFA);   // EDX
            writer.Write((byte)0x00);   // 0
            writer.Write((byte)0x90);   // NOP
            writer.Write((byte)0x90);   // NOP
            writer.Write((byte)0x90);   // NOP
        }

        public void ApplyMultipleInstancesPatch()
        {
            CheckIfDisposed();

            stream.Position = clientVersion.MultipleInstancePatchAddress;

            writer.Write((byte)0x31); // XOR
            writer.Write((byte)0xC0); // EAX, EAX
            writer.Write((byte)0x90); // NOP
            writer.Write((byte)0x90); // NOP
            writer.Write((byte)0x90); // NOP
            writer.Write((byte)0x90); // NOP
        }

        public void ApplyHideWallsPatch()
        {
            CheckIfDisposed();

            stream.Position = clientVersion.HideWallsPatchAddress;
                
            writer.Write((byte)0xEB);   // JMP SHORT
            writer.Write((byte)0x17);   // +17
            writer.Write((byte)0x90);   // NOP
        }
        #endregion

        #region IDisposable Methods
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        void Dispose(bool isDisposing)
        {
            if (isDisposed)
                return;

            if (isDisposing)
            {
                if (writer != null)
                    writer.Dispose();
            }

            isDisposed = true;
        }

        void CheckIfDisposed()
        {
            if (isDisposed)
                throw new ObjectDisposedException(this.GetType().Name);
        }
        #endregion
    }
}
