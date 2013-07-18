using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Lotus.Authentification;

namespace Lotus
{
    public class HostLoginRequestHandler : DataHandler
    {
        #region DataHandler Member
        public override Type DataType
        {
            get { return typeof(LoginRequest); }
        }
        public override void OnRecieveData(NetworkObject networkObject, IConnection connection, object data)
        {
            LoginRequest request = data as LoginRequest;
            ISecurityContext context = networkObject as ISecurityContext;

            User user = context.SecurityManager.GetUserByKey(request.Key);
            if (user != null)
            {
                connection.Username = user.Username;
                connection.Group = user.Group;
            }
            networkObject.Send(connection, connection.Group);
        }
        #endregion
    }
}
