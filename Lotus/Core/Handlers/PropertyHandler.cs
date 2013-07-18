using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;

namespace Lotus
{
    public class PropertyHandler : DataHandler
    {
        #region DataHandler Member
        public override Type DataType
        {
            get { return typeof(IPropertyAction); }
        }
        public override void OnRecieveData(NetworkObject networkObject, IConnection connection, object data)
        {
            IExecutionContext context = networkObject as IExecutionContext;
            if (data is NetworkPropertySetter)
            {
                NetworkPropertySetter setter = data as NetworkPropertySetter;
                NetworkMethod netMethod = new NetworkMethod()
                {
                    Method = "set_" + setter.Name,
                    Arguments = new object[] { setter.Value },
                    ExpectsReturnValue = false
                };
                networkObject.InvokeReceivedData(connection, new ReceivedDataEventArgs(netMethod));
            }
            if (data is NetworkPropertyGetter)
            {
                NetworkPropertyGetter getter = data as NetworkPropertyGetter;
                NetworkMethod netMethod = new NetworkMethod()
                {
                    Method = "get_" + getter.Name,
                    ExpectsReturnValue = true
                };
                networkObject.InvokeReceivedData(connection, new ReceivedDataEventArgs(netMethod));
            }
        }
        #endregion
    }
}
