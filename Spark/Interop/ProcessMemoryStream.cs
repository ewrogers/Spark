using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Spark.Interop
{
    public class ProcessMemoryStream : Stream
    {
        bool isDisposed;
        long position = 0x400000;   // Most processes will map to this base address
        ProcessAccess processAccess;

        #region Stream Properties
        public override bool CanRead
        {
            get
            {
                CheckIfDisposed();
                return processAccess.HasFlag(ProcessAccess.Read);
            }
        }

        public override bool CanSeek
        {
            get
            {
                CheckIfDisposed();
                return true;
            }
        }

        public override bool CanWrite
        {
            get
            {
                CheckIfDisposed();
                return processAccess.HasFlag(ProcessAccess.Write);
            }
        }

        public override long Length
        {
            get { throw new NotSupportedException("Process memory stream does not have a specific length"); }
        }

        public override long Position
        {
            get { return position; }
            set
            {
                CheckIfDisposed();

                if (value < 0)
                    throw new ArgumentOutOfRangeException("Position must be a positive value");

                position = value;
            }
        }
        #endregion

        public ProcessMemoryStream(int processID, ProcessAccess desiredAccess = ProcessAccess.ReadWrite)
        {
             // TODO: open process via Win32 calls
        }

        #region Stream Methods
        public override void Close()
        {
            CheckIfDisposed();
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
            throw new NotImplementedException();
        }

        public override int ReadByte()
        {
            CheckIfDisposed();
            return base.ReadByte();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            CheckIfDisposed();
            throw new NotImplementedException();
        }

        public override void SetLength(long value)
        {
            CheckIfDisposed();
            throw new NotSupportedException("Cannot set length of process memory stream");
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            CheckIfDisposed();
            throw new NotImplementedException();
        }

        public override void WriteByte(byte value)
        {
            CheckIfDisposed();
            base.WriteByte(value);
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

            Close();

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
