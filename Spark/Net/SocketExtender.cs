using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Spark.Net
{
    // This extension allows Sockets to use the Task asynchronous patterns instead of traditional Begin/End callbacks
    public static class SocketExtender
    {
        public static Task<Socket> AcceptAsync(this Socket socket)
        {
            if (socket == null)
                throw new ArgumentNullException("socket");

            var tcs = new TaskCompletionSource<Socket>();

            socket.BeginAccept(result =>
            {
                try
                {
                    var s = result.AsyncState as Socket;
                    var client = s.EndAccept(result);

                    tcs.SetResult(client);
                }
                catch (Exception ex)
                {
                    tcs.SetException(ex);
                }

            }, socket);

            return tcs.Task;
        }

        public static Task ConnectAsync(this Socket socket, IPAddress ipAddress, int port)
        {
            if (ipAddress == null)
                throw new ArgumentNullException("ipAddress");

            if (port <= 0)
                throw new ArgumentOutOfRangeException("Port must be greater than zero");

            return ConnectAsync(socket, new IPEndPoint(ipAddress, port));
        }

        public static Task ConnectAsync(this Socket socket, IPEndPoint endpoint)
        {
            if (socket == null)
                throw new ArgumentNullException("socket");

            var tcs = new TaskCompletionSource<bool>();

            socket.BeginConnect(endpoint, result =>
            {
                try
                {
                    var s = result.AsyncState as Socket;
                    s.EndConnect(result);

                    tcs.SetResult(s.Connected);
                }
                catch (Exception ex)
                {
                    tcs.SetException(ex);
                }

            }, socket);

            return tcs.Task;
        }

        public static Task DisconnectAsync(this Socket socket, bool reuseSocket = false)
        {
            if (socket == null)
                throw new ArgumentNullException("socket");

            var tcs = new TaskCompletionSource<bool>();

            socket.BeginDisconnect(reuseSocket, result =>
            {
                try
                {
                    var s = result.AsyncState as Socket;
                    s.EndDisconnect(result);

                    tcs.SetResult(!s.Connected);
                }
                catch (Exception ex)
                {
                    tcs.SetException(ex);
                }

            }, socket);

            return tcs.Task;
        }

        public static Task<int> ReceiveAsync(this Socket socket, byte[] buffer, int offset, int count, SocketFlags flags = SocketFlags.None)
        {
            if (socket == null)
                throw new ArgumentNullException("socket");

            var tcs = new TaskCompletionSource<int>();

            socket.BeginReceive(buffer, offset, count, flags, result =>
            {
                try
                {
                    var s = result.AsyncState as Socket;
                    var numberOfBytesReceived = s.EndReceive(result);

                    tcs.SetResult(numberOfBytesReceived);
                }
                catch (Exception ex)
                {
                    tcs.SetException(ex);
                }

            }, socket);

            return tcs.Task;
        }

        public static Task<int> SendAsync(this Socket socket, byte[] buffer, int offset, int count, SocketFlags flags = SocketFlags.None)
        {
            if (socket == null)
                throw new ArgumentNullException("socket");

            var tcs = new TaskCompletionSource<int>();

            socket.BeginSend(buffer, offset, count, flags, result =>
            {
                try
                {
                    var s = result.AsyncState as Socket;
                    var numberOfBytesSent = s.EndSend(result);

                    tcs.SetResult(numberOfBytesSent);
                }
                catch (Exception ex)
                {
                    tcs.SetException(ex);
                }

            }, socket);

            return tcs.Task;
        }
    }
}
