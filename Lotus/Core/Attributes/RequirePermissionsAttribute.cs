using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Lotus.Authentification;

namespace Lotus
{
    public class RequirePermissionsAttribute : Attribute
    {
        #region Constructors
        public RequirePermissionsAttribute(string permissionName)
        {
            this.PermissionName = permissionName;
        }
        #endregion

        #region Properties
        public string PermissionName { get; set; }
        #endregion
    }
}
