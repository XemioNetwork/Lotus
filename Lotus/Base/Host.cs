using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using System.Threading;
using Lotus;
using Lotus.Serialization;
using Lotus.Authentification;

namespace Lotus
{
    public class Host<TContainer> : NetworkHost, IExecutionContext, ISecurityContext
    {
        #region Constructors
        public Host(int port)
        {
            this.Start(port);
        }
        public Host() { }
        public Host(IHost host, ISerializer serializer) : base(host, serializer) { }
        #endregion

        #region Fields
        private TContainer container;
        private Dictionary<string, MethodBase> localMethods;

        private object thisLock = new object();
        #endregion

        #region Properties
        public object Container
        {
            get { return this.container; }
        }
        public Dictionary<string, MethodBase> LocalMethods
        {
            get { return this.localMethods; }
        }
        public SecurityManager SecurityManager { get; set; }
        #endregion

        #region Events
        public event EventHandler<ClientEventArgs> ClientAuthentificated;
        #endregion

        #region Event Handlers
        void OnReceivedData(object sender, ReceivedDataEventArgs e)
        {
            if (e.Data is LoginRequest)
            {
                this.InvokeClientAuthentificated((IConnection)sender);
            }
        }
        #endregion

        #region Event Invoke Methods
        private void InvokeClientAuthentificated(IConnection connection)
        {
            if (this.ClientAuthentificated != null)
            {
                this.ClientAuthentificated(this, new ClientEventArgs(connection));
            }
        }
        #endregion

        #region Methods
        public void Initialize(TContainer container)
        {
            this.container = container;

            Type type = container.GetType();
            this.localMethods = type.LoadNetworkMethods();

            this.SecurityManager = new SecurityManager();
            this.ReceivedData += new EventHandler<ReceivedDataEventArgs>(OnReceivedData);

            LocalStream.Instance.BaseHost = this;
            LocalStream.Instance.HostContainer = container;

            this.Handlers.Add(new HostLoginRequestHandler());
            this.Handlers.Add(new HostMethodHandler());
            this.Handlers.Add(new PropertyHandler());

            base.Initialize(this);
        }
        #endregion

        #region IExecutionContext Member
        public T Return<T>(IConnection connection, string remoteMethod, object[] arguments, Type[] genericTypes)
        {
            if (connection != null && connection.Connected)
            {
                NetworkMethod methodInfo = new NetworkMethod()
                {
                    Method = remoteMethod,
                    Arguments = arguments,
                    GenericArguments = genericTypes,
                    Direction = NetworkMethodDirection.ServerToClient,
                    ExpectsReturnValue = true
                };

                lock (thisLock)
                {
                    this.Send(methodInfo);
                    T returnValue = this.ReceiveObject<T>();

                    return returnValue;
                }
            }
            if (connection == null)
            {
                this.Return(connection, remoteMethod, arguments, genericTypes);
            }
            return default(T);
        }
        public void Return(IConnection connection, string remoteMethod, object[] arguments, Type[] genericTypes)
        {
            if (connection != null && connection.Connected)
            {
                NetworkMethod methodInfo = new NetworkMethod()
                {
                    Method = remoteMethod,
                    Arguments = arguments,
                    GenericArguments = genericTypes,
                    Direction = NetworkMethodDirection.ServerToClient,
                    ExpectsReturnValue = false
                };

                lock (thisLock)
                {
                    this.Send(methodInfo);
                }
            }
            if (connection == null)
            {
                for (int i = 0; i < this.Connections.Count; i++)
                {
                    this.Return(this.Connections[i], remoteMethod, arguments, genericTypes);
                }
            }
        }
        public MethodBase GetMethod(string name)
        {
            if (this.localMethods.ContainsKey(name))
            {
                return this.localMethods[name];
            }
            return null;
        }
        #endregion
    }
}
