using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
        static readonly string ServerWelcomeMessage = "CONNECTED SERVER\n";

        bool isDisposed;

        protected Socket socket;
        protected NetworkPacketBuffer packetBuffer = new NetworkPacketBuffer();
        protected byte[] receiveBuffer = new byte[BufferSize];
        protected byte[] sendBuffer = new byte[BufferSize];

        public ServerTester()
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.ReceiveTimeout = 1000;
        }

        #region IServerTester Methods
        public virtual async Task ConnectToServerAsync(IPAddress ipAddress, int port)
        {
            if (ipAddress == null)
                throw new ArgumentNullException("ipAddress");

            if (port <= 0)
                throw new ArgumentOutOfRangeException("Port must be greater than zero");

            CheckIfDisposed();

            // Connect to the server
            await socket.ConnectAsync(ipAddress, port);

            // Receive the initial welcome message packet
            var welcomePacket = await ReceiveNextPacket();
            Debug.WriteLine(string.Format("ReceivedPacket: {0}", welcomePacket));

            // Get the welcome message ASCII bytes, skipping the first ESC control character (0x1B)
            var welcomeString = Encoding.ASCII.GetString(welcomePacket.Data.Skip(1).ToArray());

            // Check that the welcome message equals the expected string value
            if (!string.Equals(welcomeString, ServerWelcomeMessage, StringComparison.Ordinal))
                throw new Exception("A service was running at the specified location, but it doesn't appear to be a Darkages server.");

            // Create a response packet
            var responsePacket = new NetworkPacket(0xAA, 0x00, 0x0A, 0x62, 0x00, 0x34, 0x00, 0x0A, 0x88, 0x6E, 0x59, 0x59, 0x75);
            var responsePacketData = responsePacket.ToArray();

            // Send the response data to the server
            await socket.SendAsync(responsePacketData, 0, responsePacketData.Length);
            Debug.WriteLine(string.Format("SentPacket: {0}", responsePacket));
        }

        public virtual async Task<bool> CheckClientVersionCodeAsync(int versionCode)
        {
            if (versionCode <= 0)
                throw new ArgumentOutOfRangeException("Version code must be greater than zero");

            CheckIfDisposed();

            // Create a client version packet
            var version = (short)versionCode;
            var versionPacket = new NetworkPacket(0xAA, 0x00, 0x06, 0x00, version.HiByte(), version.LoByte(), 0x4C, 0x4B, 0x00);
            var versionPacketData = versionPacket.ToArray();

            // Send the version packet to the server
            await socket.SendAsync(versionPacketData, 0, versionPacketData.Length);
            Debug.WriteLine(string.Format("SentPacket: {0}", versionPacket));

            // Receive the response from the server
            var responsePacket = await ReceiveNextPacket();
            Debug.WriteLine(string.Format("ReceivedPacket: {0}", responsePacket));

            // Get the status code from the response
            var statusCode = responsePacket.Data[0];

            if (statusCode > 0)
            {
                // Get the required version and raise an exception
                var requiredVersion = IntegerExtender.MakeWord(responsePacket.Data[2], responsePacket.Data[1]);
                throw new Exception(string.Format("The server requires client version {0} or higher.", requiredVersion));
            }

            // Version was accepted by the server
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
