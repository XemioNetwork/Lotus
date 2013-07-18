using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using Lotus;
using Lotus.Authentification;

namespace Lotus
{
    public class LocalConnection : Singleton<LocalConnection>, IConnection
    {
        #region Constructors
        public LocalConnection()
        {

        }
        #endregion

        #region Fields

        #endregion

        #region Properties

        #endregion

        #region Methods

        #endregion

        #region IConnection Member
        public Stream Stream
        {
            get { return LocalStream.Instance; }
        }
        public IPAddress IP
        {
            get { return IPAddress.Any; }
        }
        public int Port
        {
            get { return 0; }
        }
        public bool Connected
        {
            get { return true; }
        }
        #endregion

        #region IConnection Member
        public string Username { get; set; }
        public Group Group
        {
            get { return Group.Admin; }
            set { /* Ignore setter */ }
        }
        #endregion
    }
}
