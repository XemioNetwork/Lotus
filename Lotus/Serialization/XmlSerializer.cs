using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using XSerializer = System.Xml.Serialization.XmlSerializer;
using Lotus;

namespace Lotus.Serialization
{
    public class XmlSerializer : ISerializer
    {
        #region ISerializer Member
        public void Serialize(Stream stream, object value)
        {
            XSerializer serializer = new XSerializer(value.GetType());
            using (MemoryStream memoryStream = new MemoryStream())
            {
                serializer.Serialize(memoryStream, value);

                BinaryWriter writer = new BinaryWriter(stream);
                writer.Write(value.GetType().ToString());
                writer.Write((int)memoryStream.Length);
                writer.Write(memoryStream.ToArray());
            }
        }
        public object Deserialize(Stream stream)
        {
            BinaryReader reader = new BinaryReader(stream);
            string typeName = reader.ReadString();
            int length = reader.ReadInt32();
            byte[] data = reader.ReadBytes(length);

            using (MemoryStream memoryStream = new MemoryStream(data))
            {
                Type type = Type.GetType(typeName);
                XSerializer serializer = new XSerializer(type);

                return serializer.Deserialize(memoryStream);
            }
        }
        public T Deserialize<T>(Stream stream)
        {
            return (T)this.Deserialize(stream);
        }
        #endregion
    }
}
