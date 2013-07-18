using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using Lotus;
using Lotus.Serialization;

namespace Lotus
{
    public class NetworkClient : NetworkObject, IClient
    {
        #region Constructors
        public NetworkClient() : this(new TcpClient(), new TinySerializer()) { }
        public NetworkClient(IClient client, ISerializer serializer)
        {
            if (Network.TransferMode == TransferMode.Undefined)
            {
                this.client = client;
                this.Serializer = serializer;
            }
            else
            {
                switch (Network.Protocol)
                {
                    case Protocol.Direct:
                        this.client = new LocalClient();
                        break;
                    case Protocol.TCP:
                        this.client = new TcpClient();
                        break;
                }
            }
        }
        #endregion

        #region Fields
        private IClient client;
        private Thread workerThread;
        #endregion

        #region Methods
        private void HandleConnection(IConnection connection)
        {
            try
            {
                while (connection.Connected)
                {
                    object data = this.WaitObject();
                    this.InvokeReceivedData(connection, new ReceivedDataEventArgs(data));
                }
            }
            finally
            {
                if (this.Connected)
                {
                    this.client.Disconnect();
                }
            }
        }
        #endregion

        #region NetworkObject Member
        public override IConnection Connection
        { 
            get { return this.client.Connection; }
        }
        #endregion

        #region IClient Member
        public bool Connect(string ip, int port)
        {
            bool connected = this.client.Connect(ip, port);
            if (connected)
            {
                this.workerThread = new Thread(obj => HandleConnection(obj as IConnection));
                this.workerThread.Start(this.Connection);
            }
            return connected;
        }
        public void Disconnect()
        {
            this.workerThread.Abort();

            if (this.client.Connected)
            {
                this.client.Disconnect();
            }
        }
        public bool Connected
        {
            get { return this.client.Connected; }
        }
        public void Initialize(NetworkClient baseClient) { this.client.Initialize(baseClient); }
        #endregion
    }
}
