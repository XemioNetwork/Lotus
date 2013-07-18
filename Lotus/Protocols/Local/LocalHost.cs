using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using Lotus;

namespace Lotus
{
    public class LocalHost : IHost
    {
        #region Properties
        public NetworkHost BaseHost { get; set; }
        #endregion

        #region IHost Member
        public void Start(int port) { }
        public void Stop() { }
        public IConnection AcceptClient()
        {
            //Just do nothing until the basehost shuts down
            while (this.BaseHost != null && this.BaseHost.Active)
            {
                Thread.Sleep(1000);
            }
            return LocalConnection.Instance;
        }
        public List<IConnection> Connections
        {
            get { return new List<IConnection>() { LocalConnection.Instance }; }
        }
        public int Port
        {
            get { return 0; }
        }
        public void Initialize(NetworkHost baseHost)
        {
            this.BaseHost = baseHost;
        }
        #endregion
    }
}
