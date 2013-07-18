using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Lotus.Authentification;

namespace Lotus.LocalTest
{
    public class ServiceHost : HostBase<IService>, IService
    {
        #region Constructors
        public ServiceHost()
        {
            base.Initialize(this);
            base.Host.Start(8000);
        }
        #endregion

        #region IService Member
        [NetworkMethod]
        [RequirePermissions("A")]
        public void DoSomething()
        {
            Console.WriteLine("Hallo Welt");
        }
        [NetworkMethod]
        [RequirePermissions("B")]
        public string SayHello(string a)
        {
            return "Hello " + a;
        }
        [NetworkProperty]
        public string Test { get; set; }
        #endregion
    }
}
