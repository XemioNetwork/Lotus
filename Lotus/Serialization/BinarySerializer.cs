using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Lotus;

namespace Lotus.Serialization
{
    public class BinarySerializer : ISerializer
    {
        #region Constructors
        public BinarySerializer()
        {
            this.formatter = new BinaryFormatter();
        }
        #endregion

        #region Fields
        private BinaryFormatter formatter;
        #endregion

        #region ISerializer Member
        public void Serialize(Stream stream, object value)
        {
            this.formatter.Serialize(stream, value);
        }
        public object Deserialize(Stream stream)
        {
            return this.formatter.Deserialize(stream);
        }
        public T Deserialize<T>(Stream stream)
        {
            return (T)this.Deserialize(stream);
        }
        #endregion
    }
}
