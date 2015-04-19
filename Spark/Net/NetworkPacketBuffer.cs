using System;
using System.Collections.Generic;
using System.Text;

using Spark.Common;

namespace Spark.Net
{
    public sealed class NetworkPacketBuffer
    {
        static readonly int BufferSize = 4096;          // 4KB

        List<byte> buffer = new List<byte>(BufferSize);
        Queue<INetworkPacket> packets = new Queue<INetworkPacket>();

        #region Properties
        public int PacketCount { get { return packets.Count; } }
        #endregion

        public NetworkPacketBuffer() { }

        #region Enqueue Methods
        public void Enqueue(byte value)
        {
            buffer.Add(value);
            ProcessPacketBuffer();
        }

        public void Enqueue(IEnumerable<byte> values)
        {
            if (values == null)
                throw new ArgumentNullException("values");

            buffer.AddRange(values);
            ProcessPacketBuffer();
        }
        #endregion

        #region Dequeue Methods
        public INetworkPacket PeekPacket()
        {
            return packets.Peek();
        }

        public INetworkPacket DequeuePacket()
        {
            return packets.Dequeue();
        }
        #endregion

        void ProcessPacketBuffer()
        {
            // Process buffer while there is at least one possible packet
            while (buffer.Count > NetworkPacket.HeaderSize)
            {
                // Get the expected size of the packet
                var size = IntegerExtender.MakeWord(buffer[2], buffer[1]);
                var packetSize = size + 2;

                // If the entire packet has not been buffered, stop processing until more data arrives
                if (packetSize > buffer.Count)
                    break;

                // Copy packet from buffer
                var dataOffset = 4;
                var dataSize = packetSize - dataOffset;
                var packet = new NetworkPacket(buffer[0], size, buffer[3], buffer.GetRange(dataOffset, dataSize));

                // Queue packet and remove bytes from buffer
                packets.Enqueue(packet);
                buffer.RemoveRange(0, packetSize);
            }
        }
    }
}
