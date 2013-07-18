using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using Lotus.Authentification;

namespace Lotus
{
    public class HostMethodHandler : DataHandler
    {
        #region DataHandler Member
        public override Type DataType
        {
            get { return typeof(NetworkMethod); }
        }
        public override void OnRecieveData(NetworkObject networkObject, IConnection connection, object data)
        {
            IExecutionContext context = networkObject as IExecutionContext;
            ISecurityContext securityContext = networkObject as ISecurityContext;
            NetworkMethod netMethod = data as NetworkMethod;

            if (context.LocalMethods.ContainsKey(netMethod.Method))
            {
                RequirePermissionsAttribute attribute = null;
                MethodBase method = context.LocalMethods[netMethod.Method];

                object[] attributes = method.GetCustomAttributes(true);
                foreach (object attributeObject in attributes)
                {
                    if (attributeObject is RequirePermissionsAttribute)
                    {
                        attribute = attributeObject as RequirePermissionsAttribute;
                        break;
                    }
                }
                if (attribute == null)
                {
                    attribute = new RequirePermissionsAttribute(Permissions.None);
                }
                if (connection.Group.HasPermission(attribute.PermissionName) ||
                    connection.Group.IsAdmin ||
                    Network.SecurityMode == SecurityMode.None)
                {
                    if (method.CountParameters() == netMethod.Arguments.Length)
                    {
                        if (method.IsGenericMethod)
                        {
                            method = (method as MethodInfo).MakeGenericMethod(netMethod.GenericArguments);
                        }

                        object returnValue = method.Invoke(context.Container, netMethod.Arguments);
                        if (netMethod.ExpectsReturnValue)
                        {
                            networkObject.Send(connection, returnValue);
                        }
                    }
                    else { networkObject.SendNull(connection); }
                }
                else
                {
                    networkObject.SendNull(connection);
                    networkObject.Send(connection, new Exception("Failed to execute '" + method.Name + "'. You don't have permissions to execute this method."));
                }
            }
        }
        #endregion
    }
}
