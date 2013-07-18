using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Compression;
using Lotus;

namespace Lotus.Serialization
{
    public class CompressedSerializer<TSerializer> : ISerializer where TSerializer : ISerializer, new()
    {
        #region Constructors
        public CompressedSerializer()
        {
            this.serializer = new TSerializer();
        }
        #endregion

        #region Fields
        private ISerializer serializer;
        #endregion

        #region Static Methods
        static byte[] Compress(byte[] data)
        {
            using (var compressedStream = new MemoryStream())
            {
                using (var zipStream = new GZipStream(compressedStream, CompressionMode.Compress))
                {
                    zipStream.Write(data, 0, data.Length);
                    zipStream.Close();
                    return compressedStream.ToArray();
                }
            }
        }
        static byte[] Decompress(byte[] data)
        {
            using (var compressedStream = new MemoryStream(data))
            {
                using (var zipStream = new GZipStream(compressedStream, CompressionMode.Decompress))
                {
                    using (var resultStream = new MemoryStream())
                    {
                        zipStream.CopyTo(resultStream);
                        return resultStream.ToArray();
                    }
                }
            }
        }
        #endregion

        #region ISerializer Member
        public void Serialize(Stream stream, object value)
        {
            BinaryWriter writer = new BinaryWriter(stream);
            MemoryStream objectStream = new MemoryStream();

            this.serializer.Serialize(objectStream, value);

            bool isCompressed = objectStream.Length > 256;
            writer.Write(isCompressed);

            if (isCompressed)
            {
                byte[] data = Compress(objectStream.ToArray());
                
                writer.Write(data.Length);
                writer.Write(data);
            }
            else
            {
                writer.Write(objectStream.ToArray());
            }
        }
        public object Deserialize(Stream stream)
        {
            BinaryReader reader = new BinaryReader(stream);

            bool isCompressed = reader.ReadBoolean();
            if (isCompressed)
            {
                int length = reader.ReadInt32();
                byte[] compressedData = reader.ReadBytes(length);

                byte[] data = Decompress(compressedData);
                using (MemoryStream memoryStream = new MemoryStream(data))
                {
                    return this.serializer.Deserialize(memoryStream);
                }
            }
            else
            {
                return this.serializer.Deserialize(stream);
            }
        }
        public T Deserialize<T>(Stream stream)
        {
            return (T)this.Deserialize(stream);
        }        
        #endregion
    }
}
