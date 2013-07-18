using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Lotus.Authentification
{
    public class User
    {
        #region Constructors
        public User() { }
        public User(string username, string password) : this(username, password, Group.User) { }
        public User(string username, string password, Group group)
        {
            this.Username = username;
            this.Password = password;
            this.Group = group;
        }
        #endregion

        #region Properties
        public string Username { get; set; }
        public string Password { get; set; }
        public Group Group { get; set; }
        #endregion

        #region Methods
        public string GenerateKey()
        {
            string input = this.Username + this.Password;
            return input.Md5();
        }
        #endregion
    }
}
