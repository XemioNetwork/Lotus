using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Lotus
{
    public class FluentProperty
    {
        #region Constructors
        public FluentProperty(string propertyName, NetworkObject networkObject)
        {
            this.propertyName = propertyName;
            this.networkObject = networkObject;
        }
        #endregion

        #region Fields
        private string propertyName;
        private NetworkObject networkObject;
        #endregion

        #region Methods
        public void Set(object value)
        {
            NetworkPropertySetter setter = new NetworkPropertySetter(this.propertyName, value);
            this.networkObject.Send(setter);
        }
        public T Get<T>()
        {
            NetworkPropertyGetter getter = new NetworkPropertyGetter(this.propertyName);
            this.networkObject.Send(getter);

            return this.networkObject.ReceiveObject<T>();
        }
        #endregion
    }
}
