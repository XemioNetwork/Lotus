using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Lotus
{
    public interface ISerializable
    {
        void Serialize(BinaryWriter writer, ISerializer serializer);
        object Deserialize(BinaryReader reader, ISerializer serializer);
    }
}
