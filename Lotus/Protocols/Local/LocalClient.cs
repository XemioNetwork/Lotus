using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Lotus;

namespace Lotus
{
    public class LocalClient : IClient
    {
        #region Properties
        public NetworkClient BaseClient { get; set; }
        #endregion

        #region IClient Member
        public bool Connect(string ip, int port) 
        { 
            return false; 
        }
        public void Disconnect() { }
        public bool Connected
        {
            get { return true; }
        }
        public IConnection Connection
        {
            get { return LocalConnection.Instance; }
        }
        public void Initialize(NetworkClient baseClient)
        {
            this.BaseClient = baseClient;
        }
        #endregion
    }
}
