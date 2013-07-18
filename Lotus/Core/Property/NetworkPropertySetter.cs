using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Lotus
{
    [Serializable]
    public class NetworkPropertySetter : ISerializable, IPropertyAction
    {
        #region Constructors
        public NetworkPropertySetter() { }
        public NetworkPropertySetter(string name, object value)
        {
            this.Name = name;
            this.Value = value;
        }
        #endregion

        #region Properties
        public string Name { get; set; }
        public object Value { get; set; }
        #endregion

        #region ISerializable Member
        public void Serialize(BinaryWriter writer, ISerializer serializer)
        {
            writer.Write(this.Name);
            serializer.Serialize(writer.BaseStream, this.Value);
        }
        public object Deserialize(BinaryReader reader, ISerializer serializer)
        {
            this.Name = reader.ReadString();
            this.Value = serializer.Deserialize(reader.BaseStream);

            return this;
        }
        #endregion
    }
}
