using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Lotus.Authentification;

namespace Lotus
{
    public interface ISecurityContext
    {
        SecurityManager SecurityManager { get; set; }
    }
}
