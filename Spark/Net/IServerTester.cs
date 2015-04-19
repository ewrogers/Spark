using System;
using System.Net;
using System.Threading.Tasks;

namespace Spark.Net
{
    public interface IServerTester : IDisposable
    {
        Task ConnectToServerAsync(IPAddress ipAddress, int port);
        Task<bool> CheckClientVersionCodeAsync(int versionCode);
    }
}
