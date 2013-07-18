using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;

namespace Lotus
{
    public class ClientMethodHandler : DataHandler
    {
        #region DataHandler Member
        public override Type DataType
        {
            get { return typeof(NetworkMethod); }
        }
        public override void OnRecieveData(NetworkObject networkObject, IConnection connection, object data)
        {
            NetworkMethod netMethod = data as NetworkMethod;
            Client<object> client = networkObject as Client<object>;

            if (client.LocalMethods.ContainsKey(netMethod.Method))
            {
                MethodBase method = client.LocalMethods[netMethod.Method];
                if (method.GetParameters().Length == netMethod.Arguments.Length)
                {
                    if (method.IsGenericMethod)
                    {
                        method = (method as MethodInfo).MakeGenericMethod(netMethod.GenericArguments);
                    }

                    object returnValue = method.Invoke(client.Container, netMethod.Arguments);

                    if (netMethod.ExpectsReturnValue)
                    {
                        client.Send(returnValue);
                    }
                }
                else
                {
                    client.SendNull();
                }
            }
        }
        #endregion
    }
}
