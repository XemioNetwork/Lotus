using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Lotus;
using Lotus.Authentification;

namespace Lotus.LocalTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Network.Protocol = Protocol.TCP;
            Network.TransferMode = TransferMode.Default;

            #region Initialization
            ServiceHost serviceHost = new ServiceHost();
            ServiceClient serviceClient = new ServiceClient();
            
            SecurityManager securityManager = serviceHost.Host.SecurityManager;
            securityManager.Users.Add(new User("Admin", "1234", Group.Admin));

            serviceClient.Client.Authentificate("Admin", "1234");
            #endregion

            float elapsed = 0f;

            //Testcode
            for (int i = 0; i < 1000; i++)
            {
                Stopwatch watch = Stopwatch.StartNew();
                string a = serviceClient.SayHello("World");
                watch.Stop();

                elapsed += watch.Elapsed.Ticks / (float)TimeSpan.TicksPerMillisecond;
            }
            Console.WriteLine(serviceClient.Test);
            Console.WriteLine("{0}ms", elapsed / 1000f);
            Console.ReadLine();

            serviceHost.Host.Stop();
        }
    }
}
