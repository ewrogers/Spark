using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;

namespace Spark.Models
{
    class BasicVersionTester : IVersionTester
    {
        private Socket socket;

        public BasicVersionTester()
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        public void ConnectToServer(IPAddress ipaddr, int serverPort)
        {
            socket.Connect(ipaddr, serverPort);
            Debug.WriteLine(string.Format("Connected to {0}:{1} successfully", ipaddr, serverPort));

            // Test if the server that responded is likely a DA server
            string emsg = "A service was running at the specified location, but it doesn't appear to be a Darkages server.";
            byte[] data = new byte[1024];
            socket.ReceiveTimeout = 1000;
            int actual = socket.Receive(data, 4, SocketFlags.None);
            if (actual != 4)
                throw new Exception(emsg);
            int len = data[1] * 256 + data[2];
            if (len > 1024)
                throw new Exception(emsg);
            actual = socket.Receive(data, len - 1, SocketFlags.None);
            if (actual != len - 1 || actual < 17)
                throw new Exception(emsg);
            string welcomeMessage = System.Text.Encoding.ASCII.GetString(data, 1, 16);
            if (!welcomeMessage.Equals("CONNECTED SERVER"))
                throw new Exception(emsg);

            // First message
            byte[] firstData = { 0xAA, 0x00, 0x0A, 0x62, 0x00, 0x34, 0x00, 0x0A, 0x88, 0x6E, 0x59, 0x59, 0x75 };
            socket.Send(firstData);
        }

        public bool TestVersionNumber(int versionNumber, out int reqVersionNumber)
        {
            byte[] versionData = { 0xAA, 0x00, 0x06, 0x00, (byte)(versionNumber / 256), (byte)(versionNumber % 256), 0x4C, 0x4B, 0 };
            socket.Send(versionData);

            // Check if server accepts this version number
            byte[] data = new byte[1024];
            int actual = socket.Receive(data, 4, SocketFlags.None);

            reqVersionNumber = -1;

            if (actual != 4 || data[3] != 0)
                return false;
            int len = data[1] * 256 + data[2];
            if (len > 1024)
                return false;
            actual = socket.Receive(data, len - 1, SocketFlags.None);
            if (actual != len - 1)
                return false;
            if (data[0] == 2)
            {
                reqVersionNumber = data[1] * 256 + data[2];
                return false;
            }
            else if (data[0] == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Dispose()
        {
            socket.Dispose();
        }

    }
}
