using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using Lotus;

namespace Lotus.Serialization
{
    public enum DataType
    {
        Int16,
        Int32,
        Int64,

        String,
        Char,

        Single,
        Double,
        Decimal,

        Boolean,
        Byte
    }
    public class TinySerializer : ISerializer
    {
        #region Constructors
        public TinySerializer()
        {
            this.serializer = new BinarySerializer();
        }
        #endregion

        #region Fields
        private BinarySerializer serializer;
        #endregion

        #region Methods
        public bool IsDefaultDataType(Type type)
        {
            return
                type == typeof(Int16) ||
                type == typeof(Int32) ||
                type == typeof(Int64) ||
                type == typeof(String) ||
                type == typeof(Char) ||
                type == typeof(Single) ||
                type == typeof(Double) ||
                type == typeof(Decimal) ||
                type == typeof(Boolean) ||
                type == typeof(Byte);
        }
        public void WriteDefaultType(BinaryWriter writer, object value)
        {
            Type type = value.GetType();
            if (type == typeof(Int16))
            {
                writer.Write((byte)DataType.Int16);
                writer.Write((Int16)value);
            }
            else if (type == typeof(Int32))
            {
                writer.Write((byte)DataType.Int32);
                writer.Write((Int32)value);
            }
            else if (type == typeof(Int64))
            {
                writer.Write((byte)DataType.Int64);
                writer.Write((Int64)value);
            }
            else if (type == typeof(String))
            {
                writer.Write((byte)DataType.String);
                writer.Write((String)value);
            }
            else if (type == typeof(Char))
            {
                writer.Write((byte)DataType.Char);
                writer.Write((Char)value);
            }
            else if (type == typeof(Single))
            {
                writer.Write((byte)DataType.Single);
                writer.Write((Single)value);
            }
            else if (type == typeof(Double))
            {
                writer.Write((byte)DataType.Double);
                writer.Write((Double)value);
            }
            else if (type == typeof(Decimal))
            {
                writer.Write((byte)DataType.Decimal);
                writer.Write((Decimal)value);
            }
            else if (type == typeof(Boolean))
            {
                writer.Write((byte)DataType.Boolean);
                writer.Write((Boolean)value);
            }
            else if (type == typeof(Byte))
            {
                writer.Write((byte)DataType.Byte);
                writer.Write((Byte)value);
            }
        }
        public object ReadDefaultType(BinaryReader reader)
        {
            DataType dataType = (DataType)reader.ReadByte();
            switch (dataType)
            {
                case DataType.Int16: return reader.ReadInt16();
                case DataType.Int32: return reader.ReadInt32();
                case DataType.Int64: return reader.ReadInt64();
                case DataType.String: return reader.ReadString();
                case DataType.Char: return reader.ReadChar();
                case DataType.Single: return reader.ReadSingle();
                case DataType.Double: return reader.ReadDouble();
                case DataType.Decimal: return reader.ReadDecimal();
                case DataType.Boolean: return reader.ReadBoolean();
                case DataType.Byte: return reader.ReadByte();
                default: return null;
            }
        }
        #endregion

        #region ISerializer Member
        public void Serialize(Stream stream, object value)
        {
            BinaryWriter writer = new BinaryWriter(stream);
            if (value != null)
            {
                Type type = value.GetType();

                writer.Write(value is ISerializable);
                writer.Write(type.IsArray);

                if (type.IsArray)
                {
                    Array array = (Array)value;
                    writer.Write(array.Length);

                    foreach (object field in array)
                    {
                        this.Serialize(stream, field);
                    }
                }
                else
                {
                    if (value is ISerializable)
                    {
                        writer.Write(type.ToString());

                        (value as ISerializable).Serialize(writer, this);
                    }
                    else
                    {
                        bool isDefaultType = this.IsDefaultDataType(type);
                        writer.Write(isDefaultType);

                        switch (isDefaultType)
                        {
                            case true:
                                this.WriteDefaultType(writer, value);
                                break;
                            case false:
                                this.serializer.Serialize(stream, value);
                                break;
                        }
                    }
                }
            }
        }
        public object Deserialize(Stream stream)
        {
            BinaryReader reader = new BinaryReader(stream);

            bool isSerializable = reader.ReadBoolean();
            bool isArray = reader.ReadBoolean();

            if (isArray)
            {
                int arrayLength = reader.ReadInt32();
                object[] result = new object[arrayLength];

                for (int i = 0; i < arrayLength; i++)
                {
                    result[i] = this.Deserialize(stream);
                }
                return result;
            }
            else
            {
                if (isSerializable)
                {
                    string typeName = reader.ReadString();
                    Type type = Type.GetType(typeName);

                    ISerializable serializable = (ISerializable)Activator.CreateInstance(type);
                    return serializable.Deserialize(reader, this);
                }
                else
                {
                    bool isDefaultType = reader.ReadBoolean();
                    switch (isDefaultType)
                    {
                        case true: return this.ReadDefaultType(reader);
                        case false: return this.serializer.Deserialize(stream);
                    }
                }
            }
            return null;
        }
        public T Deserialize<T>(Stream stream)
        {
            return (T)this.Deserialize(stream);
        }
        #endregion
    }
}
