using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

using Spark.Common;

namespace Spark.Net
{
    public class ServerTester : IServerTester
    {
        static readonly int BufferSize = 4096;  // 4KB

        bool isDisposed;

        protected Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        protected NetworkPacketBuffer packetBuffer = new NetworkPacketBuffer();
        protected byte[] receiveBuffer = new byte[BufferSize];
        protected byte[] sendBuffer = new byte[BufferSize];

        public ServerTester() { }

        #region IServerTester Methods
        public virtual Task ConnectToServerAsync(IPAddress ipAddress, int port)
        {
            if (ipAddress == null)
                throw new ArgumentNullException("ipAddress");

            if (port <= 0)
                throw new ArgumentOutOfRangeException("Port must be greater than zero");

            CheckIfDisposed();
            return socket.ConnectAsync(ipAddress, port);
        }

        public virtual async Task<bool> CheckClientVersionCodeAsync(int versionCode)
        {
            if (versionCode <= 0)
                throw new ArgumentOutOfRangeException("Version code must be greater than zero");

            CheckIfDisposed();

            var packet = await ReceiveNextPacket();

            return true;
        }
        #endregion

        protected async Task<INetworkPacket> ReceiveNextPacket()
        {
            // If there are no available packets, receive data until one is available
            while (packetBuffer.PacketCount < 1)
            {
                var numberOfBytesReceived = await socket.ReceiveAsync(receiveBuffer, 0, receiveBuffer.Length);

                if (numberOfBytesReceived > 0)
                {
                    // Queue the received bytes to the packet buffer
                    var receivedBytes = new ArraySegment<byte>(receiveBuffer, 0, numberOfBytesReceived);
                    packetBuffer.Enqueue(receivedBytes);
                }
                else
                {
                    // The server has disconnected
                    throw new SocketException((int)SocketError.ConnectionReset);
                }
            }

            return packetBuffer.DequeuePacket();
        }

        #region IDisposable Methods
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool isDisposing)
        {
            if (isDisposed)
                return;

            if (isDisposing)
            {
                if (socket != null)
                    socket.Dispose();
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
