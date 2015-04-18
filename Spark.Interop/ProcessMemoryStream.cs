using System;
using System.IO;

using Spark.Win32;

namespace Spark.Interop
{
    public class ProcessMemoryStream : Stream
    {
        protected static readonly long ModuleBaseAddress = 0x400000;      // Most processes will map to this base address

        bool isDisposed;
        bool leaveOpen;
        Win32ProcessSafeHandle processHandle;
        byte[] readBuffer;
        byte[] writeBuffer;

        protected long position = ModuleBaseAddress;
        protected ProcessAccess processAccess;
        
        #region Stream Properties
        public override bool CanRead
        {
            get
            {
                CheckIfDisposed();
                return !processHandle.IsClosed && processAccess.HasFlag(ProcessAccess.Read);
            }
        }

        public override bool CanSeek
        {
            get
            {
                CheckIfDisposed();
                return !processHandle.IsClosed;
            }
        }

        public override bool CanWrite
        {
            get
            {
                CheckIfDisposed();
                return !processHandle.IsClosed && processAccess.HasFlag(ProcessAccess.Write);
            }
        }

        public override long Length
        {
            get { throw new NotSupportedException("Process memory stream does not have a specific length"); }
        }

        public override long Position
        {
            get
            {
                CheckIfDisposed();
                return position;
            }
            set
            {
                CheckIfDisposed();

                if (value < 0)
                    throw new ArgumentOutOfRangeException("Position must be a positive value");

                position = value;
            }
        }
        #endregion

        public ProcessMemoryStream(int processId, ProcessAccess desiredAccess = ProcessAccess.ReadWrite, int bufferSize = 4096, bool leaveOpen = false)
        {
            if (processId < 0)
                throw new ArgumentOutOfRangeException("Process ID must be a positive value");

            if (bufferSize < 1)
                throw new ArgumentOutOfRangeException("Buffer size must be at least 1 byte");

            var win32Flags = Win32ProcessAccess.VmOperation;

            // If read mode was requested, bitwise OR the flag
            if (desiredAccess.HasFlag(ProcessAccess.Read))
                win32Flags |= Win32ProcessAccess.VmRead;

            // If write mode was requested, bitwise OR the flag
            if (desiredAccess.HasFlag(ProcessAccess.Write))
                win32Flags |= Win32ProcessAccess.VmWrite;

            // Open the process and check if the handle is valid
            this.processAccess = desiredAccess;
            this.processHandle = NativeMethods.OpenProcess(win32Flags, false, processId);
            this.leaveOpen = leaveOpen;

            // Check if handle is valid
            if (this.processHandle.IsInvalid)
            {
                var errorCode = NativeMethods.GetLastError();
                throw new IOException("Unable to open process", errorCode);
            }

            // Allocate read and write buffers
            this.readBuffer = new byte[bufferSize];
            this.writeBuffer = new byte[bufferSize];
        }

        ~ProcessMemoryStream()
        {
            Dispose(false);
        }

        #region Stream Methods
        public override void Close()
        {
            CheckIfDisposed();

            processHandle.Close();
            base.Close();
        }

        public override void Flush()
        {
            CheckIfDisposed();
            // Flush has no effect on process memory streams
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            CheckIfDisposed();

            var totalBytesRead = 0;

            while (count > 0)
            {
                // Do not exceed the buffer size for each block read
                var blockSize = Math.Min(count, readBuffer.Length);

                // Read the block from process memory
                var numberOfBytesRead = 0;
                var didRead = NativeMethods.ReadProcessMemory(processHandle, (IntPtr)this.Position, readBuffer, blockSize, out numberOfBytesRead);

                // Check if the read was successful
                if (!didRead || numberOfBytesRead != blockSize)
                    throw new IOException("Unable to read block from process");

                // Copy the block from the read buffer
                Buffer.BlockCopy(readBuffer, 0, buffer, offset, blockSize);

                // Increment the offset and stream position by the number of bytes read
                offset += numberOfBytesRead;
                this.Position += numberOfBytesRead;
                totalBytesRead += numberOfBytesRead;

                // Decrement the count by the number of bytes read
                count -= numberOfBytesRead;
            }

            return totalBytesRead;
        }

        public override int ReadByte()
        {
            CheckIfDisposed();

            // Read the byte from process memory
            var numberOfBytesRead = 0;
            var didRead = NativeMethods.ReadProcessMemory(processHandle, (IntPtr)this.Position, readBuffer, 1, out numberOfBytesRead);

            // Check if the read was successful
            if (!didRead || numberOfBytesRead != 1)
                throw new IOException("Unable to read byte from process");

            // Increment the stream position by the number of bytes read
            this.Position += numberOfBytesRead;

            return readBuffer[0];
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            CheckIfDisposed();
            
            switch (origin)
            {
                case SeekOrigin.Begin:
                    this.Position = offset;
                    break;

                case SeekOrigin.Current:
                    this.Position += offset;
                    break;

                case SeekOrigin.End:
                    throw new NotSupportedException("Cannot seek from end of process memory stream");
            }

            return this.Position;
        }

        public override void SetLength(long value)
        {
            CheckIfDisposed();
            throw new NotSupportedException("Cannot set length of process memory stream");
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            CheckIfDisposed();

            while (count > 0)
            {
                // Do not exceed the buffer size for each block written
                var blockSize = Math.Min(count, writeBuffer.Length);

                // Copy the block to the write buffer
                Buffer.BlockCopy(buffer, offset, writeBuffer, 0, blockSize);

                // Write block to process memory
                var numberOfBytesWritten = 0;
                var didWrite = NativeMethods.WriteProcessMemory(processHandle, (IntPtr)this.Position, writeBuffer, blockSize, out numberOfBytesWritten);

                // Check if the write was successful
                if (!didWrite || numberOfBytesWritten != blockSize)
                    throw new IOException("Unable to write block to process");

                // Increment the offset and stream position by the number of bytes written
                offset += numberOfBytesWritten;
                this.Position += numberOfBytesWritten;

                // Decrement the count by the number of bytes written
                count -= numberOfBytesWritten;
            }
        }

        public override void WriteByte(byte value)
        {
            CheckIfDisposed();

            // Copy value to write buffer
            writeBuffer[0] = value;

            // Write byte to process memory
            var numberOfBytesWritten = 0;
            var didWrite = NativeMethods.WriteProcessMemory(processHandle, (IntPtr)this.Position, writeBuffer, 1, out numberOfBytesWritten);

            // Check if the write was successful
            if (!didWrite || numberOfBytesWritten != 1)
                throw new IOException("Unable to write byte to process");

            // Increment the stream position by the number of bytes written
            this.Position += numberOfBytesWritten;
        }
        #endregion

        #region IDisposable Methods
        protected override void Dispose(bool isDisposing)
        {
            if (isDisposed)
                return;

            if (isDisposing)
            {
                // Dispose of managed resources here
            }

            // Dispose of unmanaged resources here
            if (!leaveOpen)
                processHandle.Dispose();

            base.Dispose(isDisposing);
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
