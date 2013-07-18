using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Lotus;

namespace Lotus
{
    [Serializable]
    public class NetworkMethod : ISerializable
    {
        #region Constructors
        public NetworkMethod() 
        {
            this.Arguments = new object[] { };
            this.GenericArguments = new Type[] { };
        }
        #endregion

        #region Properties
        public string Method { get; set; }
        public object[] Arguments { get; set; }
        public Type[] GenericArguments { get; set; }
        public NetworkMethodDirection Direction { get; set; }
        public bool ExpectsReturnValue { get; set; }
        #endregion

        #region ISerializable Member
        public void Serialize(BinaryWriter writer, ISerializer serializer)
        {
            writer.Write(this.Method);

            serializer.Serialize(writer.BaseStream, this.Arguments);

            writer.Write(this.GenericArguments.Length);
            for (int i = 0; i < this.GenericArguments.Length; i++)
            {
                writer.Write(this.GenericArguments[i].ToString());
            }

            writer.Write((byte)this.Direction);
            writer.Write(this.ExpectsReturnValue);
        }
        public object Deserialize(BinaryReader reader, ISerializer serializer)
        {
            this.Method = reader.ReadString();

            this.Arguments = serializer.Deserialize<object[]>(reader.BaseStream);

            this.GenericArguments = new Type[reader.ReadInt32()];
            for (int i = 0; i < this.GenericArguments.Length; i++)
            {
                this.GenericArguments[i] = Type.GetType(reader.ReadString());
            }

            this.Direction = (NetworkMethodDirection)reader.ReadByte();
            this.ExpectsReturnValue = reader.ReadBoolean();

            return this;
        }
        #endregion
    }
    public enum NetworkMethodDirection
    {
        ClientToServer,
        ServerToClient
    }
}
