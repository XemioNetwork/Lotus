using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Lotus.LocalTest
{
    public class ServiceClient : ClientBase<IService>, IService
    {
        #region Constructors
        public ServiceClient()
        {
            base.Initialize(this);
            base.Client.Connect("127.0.0.1", 8000);
        }
        #endregion

        #region IService Member
        public void DoSomething()
        {
            base
                .Method("DoSomething")
                .Execute();
        }
        public string SayHello(string a)
        {
            return base
                .Method("SayHello")
                .WithArguments(a)
                .Return<string>();
        }
        #endregion

        #region IService Member
        public string Test
        {
            get { return base.Property("Test").Get<string>(); }
            set { base.Property("Test").Set(value); }
        }
        #endregion
    }
}
