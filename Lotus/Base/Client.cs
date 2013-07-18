using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using Lotus.Authentification;

namespace Lotus
{    
    public class Client<TContainer> : NetworkClient, IExecutionContext
    {
        #region Constructors
        public Client(string ip, int port)
        {
            this.Connect(ip, port);
        }
        public Client() { }
        public Client(IClient client, ISerializer serializer) : base(client, serializer) { }
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
        #endregion

        #region Methods
        public void Initialize(TContainer container)
        {
            this.container = container;

            Type type = container.GetType();
            this.localMethods = type.LoadNetworkMethods();

            LocalStream.Instance.BaseClient = this;
            LocalStream.Instance.ClientContainer = container;

            this.Handlers.Add(new ClientMethodHandler());
            this.Handlers.Add(new ClientExceptionHandler());
            this.Handlers.Add(new PropertyHandler());

            base.Initialize(this);
        }
        public void Authentificate(string username, string password)
        {
            if (Network.Protocol != Protocol.Direct)
            {
                LoginRequest request = new LoginRequest(username, password);

                lock (thisLock)
                {
                    this.Send(request);
                }

                this.Connection.Username = username;
                this.Connection.Group = this.ReceiveObject<Group>();
            }
        }
        public T Return<T>(string remoteMethod, object[] arguments, Type[] genericTypes)
        {
            return this.Return<T>(this.Connection, remoteMethod, arguments, genericTypes);
        }
        public void Return(string remoteMethod, object[] arguments, Type[] genericTypes)
        {
            this.Return(this.Connection, remoteMethod, arguments, genericTypes);
        }
        #endregion

        #region IExecutionContext Member
        public T Return<T>(IConnection connection, string method, object[] arguments, Type[] genericTypes)
        {
            if (connection.Connected)
            {
                NetworkMethod netMethod = new NetworkMethod()
                {
                    Method = method,
                    Arguments = arguments,
                    GenericArguments = genericTypes.ToArray(),
                    Direction = NetworkMethodDirection.ClientToServer,
                    ExpectsReturnValue = true
                };

                lock (thisLock)
                {
                    this.Send(netMethod);
                    T returnValue = this.ReceiveObject<T>();

                    return returnValue;
                }
            }
            return default(T);
        }
        public void Return(IConnection connection, string method, object[] arguments, Type[] genericTypes)
        {
            if (connection.Connected)
            {
                NetworkMethod netMethod = new NetworkMethod()
                {
                    Method = method,
                    Arguments = arguments,
                    GenericArguments = genericTypes.ToArray(),
                    Direction = NetworkMethodDirection.ClientToServer,
                    ExpectsReturnValue = false
                };

                lock (thisLock)
                {
                    this.Send(netMethod);
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
