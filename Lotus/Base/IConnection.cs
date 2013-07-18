using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using Lotus.Authentification;

namespace Lotus
{
    public interface IConnection
    {
        Stream Stream { get; }
        IPAddress IP { get; }

        int Port { get; }
        bool Connected { get; }

        string Username { get; set; }
        Group Group { get; set; }
    }
}
