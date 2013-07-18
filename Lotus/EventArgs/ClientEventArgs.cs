using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Lotus
{
    public class ClientEventArgs : EventArgs
    {
        #region Constructors
        public ClientEventArgs(IConnection client)
        {
            this.Client = client;
        }
        #endregion

        #region Properties
        public IConnection Client { get; set; }
        #endregion
    }
}
