using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Lotus
{
    public class ClientExceptionHandler : DataHandler
    {
        #region DataHandler Member
        public override Type DataType
        {
            get { return typeof(Exception); }
        }
        public override void OnRecieveData(NetworkObject networkObject, IConnection connection, object data)
        {
            throw (data as Exception);
        }
        #endregion

    }
}
