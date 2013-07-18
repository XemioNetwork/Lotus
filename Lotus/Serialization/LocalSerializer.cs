using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Lotus;

namespace Lotus.Serialization
{
    public class LocalSerializer : ISerializer
    {
        #region ISerializer Member
        public void Serialize(Stream stream, object value)
        {
            if (value is NetworkMethod && stream is LocalStream)
            {
                NetworkMethod methodInfo = value as NetworkMethod;
                LocalStream.Instance.ExecuteMethod(methodInfo);
            }
        }
        public object Deserialize(Stream stream)
        {
            return null;
        }
        public T Deserialize<T>(Stream stream)
        {
            return default(T);
        }
        #endregion
    }
}
