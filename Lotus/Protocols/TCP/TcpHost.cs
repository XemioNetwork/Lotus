using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using Sockets = System.Net.Sockets;
using Lotus;

namespace Lotus
{
    public class TcpHost : IHost
    {
        #region Constructors
        public TcpHost()
        {
            this.Connections = new List<IConnection>();
        }
        #endregion

        #region Fields
        private Sockets.TcpListener listener;
        #endregion

        #region IHost Member
        public void Start(int port)
        {
            this.Port = port;

            this.listener = new Sockets.TcpListener(IPAddress.Any, port);
            this.listener.Start();
        }
        public void Stop()
        {
            this.listener.Stop();
        }
        public IConnection AcceptClient()
        {
            Sockets.TcpClient client = this.listener.AcceptTcpClient();
            IConnection connection = new TcpConnection(client);

            this.Connections.Add(connection);
            return connection;
        }
        public List<IConnection> Connections { get; private set; }
        public int Port { get; private set; }
        public void Initialize(NetworkHost baseHost) { }
        #endregion
    }
}
