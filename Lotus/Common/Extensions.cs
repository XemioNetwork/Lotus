using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using System.Net;
using Sockets = System.Net.Sockets;
using System.Security.Cryptography;

namespace Lotus
{
    public static class Extensions
    {
        #region Static Properties
        public static int DefaultMd5Iterations 
        { 
            get { return 5; }
        }
        #endregion

        #region Static Methods
        public static string Md5(this string self, int iterations)
        {
            MD5 md5 = MD5.Create();
            byte[] data = new byte[self.Length];

            data = Encoding.Default.GetBytes(self);

            for (int i = 0; i < iterations; i++)
            {
                data = md5.ComputeHash(data);
            }

            StringBuilder output = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                output.Append(data[i].ToString("X2"));
            }

            return output.ToString().ToLower();
        }
        public static string Md5(this string self)
        {
            return self.Md5(Extensions.DefaultMd5Iterations);
        }
        public static IPAddress GetAddress(this Sockets.TcpClient client)
        {
            return client.Client.RemoteEndPoint.GetAddress();
        }
        public static IPAddress GetAddress(this EndPoint endPoint)
        {
            return (endPoint as IPEndPoint).Address;
        }
        public static int GetPort(this Sockets.TcpClient client)
        {
            return client.Client.RemoteEndPoint.GetPort();
        }
        public static int GetPort(this EndPoint endPoint)
        {
            return (endPoint as IPEndPoint).Port;
        }
        public static int CountParameters(this MethodBase methodBase)
        {
            return methodBase.GetParameters().Length;
        }
        public static Dictionary<string, MethodBase> LoadNetworkMethods(this Type type)
        {
            Dictionary<string, MethodBase> methods = new Dictionary<string, MethodBase>();
            foreach (MethodBase method in type.GetMethods())
            {
                foreach (object attr in method.GetCustomAttributes(true))
                {
                    if (attr is NetworkMethodAttribute)
                    {
                        methods.Add(method.Name, method);
                    }
                }
            }
            foreach (PropertyInfo property in type.GetProperties())
            {
                foreach (object attr in property.GetCustomAttributes(true))
                {
                    if (attr is NetworkPropertyAttribute)
                    {
                        MethodInfo setMethod = property.GetSetMethod();
                        MethodInfo getMethod = property.GetGetMethod();

                        methods.Add(setMethod.Name, setMethod);
                        methods.Add(getMethod.Name, getMethod);
                    }
                }
            }
            return methods;
        }
        #endregion
    }
}
