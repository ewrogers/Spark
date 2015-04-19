using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spark.Net
{
    public interface INetworkPacket : IEnumerable<byte>
    {
        byte Signature { get; }
        short Size { get; }
        byte Command { get; }
        IReadOnlyList<byte> Data { get; }
    }
}
