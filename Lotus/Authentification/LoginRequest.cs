using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Lotus;

namespace Lotus.Authentification
{
    public class LoginRequest : ISerializable
    {
        #region Constructors
        public LoginRequest() { }
        public LoginRequest(string username, string password)
        {
            User user = new User(username, password);
            this.Key = user.GenerateKey();
        }
        #endregion

        #region Properties
        public string Key { get; set; }
        #endregion

        #region ISerializable Member
        public void Serialize(BinaryWriter writer, ISerializer serializer)
        {
            writer.Write(this.Key);
        }
        public object Deserialize(BinaryReader reader, ISerializer serializer)
        {
            this.Key = reader.ReadString();
            return this;
        }
        #endregion
    }
}
