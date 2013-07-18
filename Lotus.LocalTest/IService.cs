using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Lotus.LocalTest
{
    public interface IService
    {
        void DoSomething();
        string SayHello(string a);

        string Test { get; set; }
    }
}
