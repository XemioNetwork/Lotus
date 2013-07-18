using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using Sockets = System.Net.Sockets;

namespace Lotus
{
    public class TcpClient : IClient
    {
        #region Constructors
        public TcpClient()
        {
        }
        #endregion

        #region Fields
        private Sockets.TcpClient client;
        #endregion

        #region IClient Member
        public bool Connect(string ip, int port)
        {
            try
            {
                this.client = new Sockets.TcpClient();
                this.client.Connect(IPAddress.Parse(ip), port);

                if (this.client.Connected)
                {
                    this.Connection = new TcpConnection(client);
                }
            }
            catch
            {
                this.Connection = null;
            }
            return this.client.Connected;
        }
        public void Disconnect()
        {
            this.client.Client.Disconnect(false);
            this.client.Close();
        }
        public IConnection Connection { get; private set; }
        public bool Connected
        { 
            get { return this.client != null && this.client.Client != null && this.client.Client.Connected; }
        }
        public void Initialize(NetworkClient baseClient) { }
        #endregion
    }
}
