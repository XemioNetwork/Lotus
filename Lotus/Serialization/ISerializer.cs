using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Lotus
{
    public interface ISerializer
    {
        void Serialize(Stream stream, object value);

        object Deserialize(Stream stream);
        T Deserialize<T>(Stream stream);
    }
}
