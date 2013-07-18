using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using Lotus;
using Lotus.Serialization;
using Lotus.Authentification;

namespace Lotus
{
    public class NetworkHost : NetworkObject, IHost
    {
        #region Constructors
        public NetworkHost() : this(new TcpHost(), new TinySerializer()) { }
        public NetworkHost(IHost host, ISerializer serializer)
        {
            if (Network.TransferMode == TransferMode.Undefined)
            {
                this.host = host;
                this.Serializer = serializer;
            }
            else
            {
                switch (Network.Protocol)
                {
                    case Protocol.Direct:
                        this.host = new LocalHost();
                        break;
                    case Protocol.TCP:
                        this.host = new TcpHost();
                        break;
                }
            }

            this.Active = true;
        }
        #endregion

        #region Fields
        private IHost host;
        private IConnection connection;

        private List<Thread> serverThreads = new List<Thread>();
        #endregion

        #region Properties
        public bool Active { get; set; }
        #endregion

        #region Events
        public event EventHandler<ClientEventArgs> ClientConnected;
        public event EventHandler<ClientEventArgs> ClientDisconnected;

        public event EventHandler ServerStarted;
        public event EventHandler ServerStopped;
        #endregion

        #region Methods
        public void StartAcceptClientsAsync()
        {
            Thread acceptClientsThread = new Thread(() => this.AcceptClients());
            acceptClientsThread.Start();

            this.serverThreads.Add(acceptClientsThread);
        }
        private void AcceptClients()
        {
            while (this.Active)
            {
                this.StartHandleConnectionAsync(this.AcceptClient());
            }
        }
        public void StartHandleConnectionAsync(IConnection connection)
        {
            Thread handleConnectionThread = new Thread(obj => this.HandleConnection(obj as IConnection));
            handleConnectionThread.Start(connection);

            this.serverThreads.Add(handleConnectionThread);
        }
        private void HandleConnection(IConnection connection)
        {
            try
            {
                connection.Group = Group.Guest;
                this.InvokeClientConnected(connection);

                while (this.Active && connection.Connected)
                {
                    object data = this.WaitObject(connection);
                    this.InvokeReceivedData(connection, new ReceivedDataEventArgs(data));
                }
            }
            finally
            {
                this.InvokeClientDisconnected(connection);
                this.Connections.Remove(connection);
            }
        }
        #endregion

        #region Event Invoke Methods
        private void InvokeClientConnected(IConnection connection)
        {
            if (this.ClientConnected != null)
            {
                this.ClientConnected(this, new ClientEventArgs(connection));
            }
        }
        private void InvokeClientDisconnected(IConnection connection)
        {
            if (this.ClientDisconnected != null)
            {
                this.ClientDisconnected(this, new ClientEventArgs(connection));
            }
        }
        private void InvokeServerStarted()
        {
            if (this.ServerStarted != null)
            {
                this.ServerStarted(this, new EventArgs());
            }
        }
        private void InvokeServerStopped()
        {
            if (this.ServerStopped != null)
            {
                this.ServerStopped(this, new EventArgs());
            }
        }
        #endregion

        #region NetworkObject Member
        public override IConnection Connection
        { 
            get { return this.connection; } 
        }        
        public override void Send(object value)
        {
            for (int i = 0; i < this.host.Connections.Count; i++)
            {
                this.Send(this.Connections[i], value);
            }
        }
        public override object WaitObject()
        {
            return this.WaitObject<object>();
        }
        public override T WaitObject<T>()
        {
            if (this.host.Connections.Count > 0)
            {
                this.connection = this.Connections[0];
                return base.WaitObject<T>();
            }
            return default(T);
        }
        public void Send(int clientId, object value)
        {
            this.Send(this.Connections[clientId], value);
        }
        public object WaitObject(int clientId)
        {
            return this.WaitObject(this.Connections[clientId]);
        }
        public T WaitObject<T>(int clientId)
        {
            return this.WaitObject<T>(this.Connections[clientId]);
        }
        #endregion

        #region IHost Member
        public void Start(int port)
        {
            this.host.Start(port);
            this.StartAcceptClientsAsync();

            this.InvokeServerStarted();
        }
        public void Stop()
        {
            this.Active = false;

            foreach (Thread thread in this.serverThreads)
            {
                if (thread.IsAlive)
                {
                    thread.Abort();
                }
            }
            this.serverThreads.Clear();

            this.host.Stop();
            this.InvokeServerStopped();
        }
        public IConnection AcceptClient()
        {
            return this.host.AcceptClient();
        }
        public List<IConnection> Connections
        {
            get { return this.host.Connections; }
        }
        public int Port
        {
            get { return this.host.Port; }
        }
        public void Initialize(NetworkHost baseHost)
        {
            this.host.Initialize(baseHost);
        }
        #endregion
    }
}
