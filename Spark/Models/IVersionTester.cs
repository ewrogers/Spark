using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace Spark.Models
{
    interface IVersionTester : IDisposable
    {
        void ConnectToServer(IPAddress ipaddr, int serverPort);

        bool TestVersionNumber(int versionNumber, out int reqVersionNumber);
    }
}
