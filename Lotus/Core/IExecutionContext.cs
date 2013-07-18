using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;

namespace Lotus
{
    public interface IExecutionContext
    {
        object Container { get; }
        Dictionary<string, MethodBase> LocalMethods { get; }

        T Return<T>(IConnection connection, string method, object[] arguments, Type[] genericTypes);
        void Return(IConnection connection, string method, object[] arguments, Type[] genericTypes);

        MethodBase GetMethod(string name);
    }
}
