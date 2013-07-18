using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Lotus
{
    public interface IClient
    {
        bool Connect(string ip, int port);
        void Disconnect();

        bool Connected { get; }
        IConnection Connection { get; }

        void Initialize(NetworkClient baseClient);
    }
}
