using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Lotus
{
    public abstract class DataHandler
    {
        #region Properties
        public abstract Type DataType { get; }
        #endregion

        #region Methods
        public abstract void OnRecieveData(NetworkObject networkObject, IConnection connection, object data);
        #endregion
    }
}
