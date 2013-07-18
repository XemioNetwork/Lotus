using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using Sockets = System.Net.Sockets;
using Lotus;
using Lotus.Authentification;

namespace Lotus
{
    public class TcpConnection : IConnection
    {
        #region Constructors
        public TcpConnection(Sockets.TcpClient client)
        {
            this.client = client;
        }
        #endregion

        #region Fields
        private Sockets.TcpClient client;
        #endregion

        #region IConnection Member
        public Stream Stream
        {
            get { return this.client.GetStream(); }
        }
        public IPAddress IP
        {
            get { return this.client.GetAddress(); }
        }
        public int Port
        {
            get { return this.client.GetPort(); }
        }
        public bool Connected
        {
            get { return this.client.Connected; }
        }
        public string Username { get; set; }
        public Group Group { get; set; }
        #endregion
    }
}
