using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Lotus
{
    [Serializable]
    public class NetworkPropertyGetter : ISerializable, IPropertyAction
    {
        #region Constructors
        public NetworkPropertyGetter() { }
        public NetworkPropertyGetter(string propertyName)
        {
            this.Name = propertyName;
        }
        #endregion

        #region Properties
        public string Name { get; set; }
        #endregion

        #region ISerializable Member
        public void Serialize(BinaryWriter writer, ISerializer serializer)
        {
            writer.Write(this.Name);
        }
        public object Deserialize(BinaryReader reader, ISerializer serializer)
        {
            this.Name = reader.ReadString();
            return this;
        }
        #endregion
    }
}
