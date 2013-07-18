using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Lotus.Authentification
{
    [Serializable]
    public class Group
    {
        #region Constructors
        public Group() 
        {
            this.Permissions = new List<string>();
            this.IsAdmin = false;
        }
        public Group(string name) : this()
        {
            this.Name = name;
        }      
        #endregion

        #region Properties
        public string Name { get; set; }
        public List<string> Permissions { get; set; }

        public bool IsAdmin { get; set; }
        #endregion

        #region Static Fields
        private static Group admin = new Group(GroupName.Admin) { IsAdmin = true };
        private static Group user = new Group(GroupName.User);
        private static Group guest = new Group(GroupName.Guest);
        #endregion

        #region Static Properties
        public static Group Admin
        {
            get { return Group.admin; }
        }
        public static Group User
        {
            get { return Group.user; }
        }
        public static Group Guest
        {
            get { return Group.guest; }
        }
        #endregion

        #region Methods
        public bool HasPermission(string permissionName)
        {
            if (permissionName == Lotus.Authentification.Permissions.None)
            {
                return true;
            }
            for (int i = 0; i < this.Permissions.Count; i++)
            {
                if (this.Permissions[i] == permissionName)
                {
                    return true;
                }
            }
            return false;
        }
        #endregion
    }
}
