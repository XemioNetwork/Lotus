using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Lotus
{
    public interface IHost
    {
        void Start(int port);
        void Stop();

        IConnection AcceptClient();
        List<IConnection> Connections { get; }

        int Port { get; }

        void Initialize(NetworkHost baseHost);
    }
}
