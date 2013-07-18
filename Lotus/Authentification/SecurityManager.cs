using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Lotus;

namespace Lotus.Authentification
{
    public class SecurityManager
    {
        #region Constructors
        public SecurityManager()
        {
            this.Groups = new List<Group>();
            this.Users = new List<User>();
        }
        #endregion

        #region Properties
        public List<Group> Groups { get; set; }
        public List<User> Users { get; set; }
        #endregion

        #region Methods
        public User GetUserByKey(string key)
        {
            return this.Users.FirstOrDefault(user => user.GenerateKey() == key);
        }
        public User GetUserByName(string username)
        {
            return this.Users.FirstOrDefault(user => user.Username == username);
        }
        public Group GetGroupByName(string name)
        {
            return this.Groups.FirstOrDefault(group => group.Name == name);
        }
        #endregion
    }
}
