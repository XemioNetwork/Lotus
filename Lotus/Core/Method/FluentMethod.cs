using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Lotus
{
    public class FluentMethod
    {
        #region Constructors
        public FluentMethod(string method, IExecutionContext context)
        {
            this.method = method;
            this.context = context;

            this.connections = new List<IConnection>();
        }
        public FluentMethod(string method, IExecutionContext context, IConnection connection) : this(method, context)
        {
            this.connections.Add(connection);
        }
        #endregion

        #region Fields
        private string method = string.Empty;
        private object[] arguments = new object[] { };
        private List<Type> genericTypes = new List<Type>();

        private IExecutionContext context;
        private List<IConnection> connections;
        #endregion

        #region Methods
        public FluentMethod WithArguments(params object[] arguments)
        {
            this.arguments = arguments; return this;
        }
        public FluentMethod WithGeneric<T>()
        {
            this.genericTypes.Add(typeof(T)); return this;
        }
        public FluentMethod AndGeneric<T>()
        {
            return this.WithGeneric<T>();
        }
        public FluentMethod WithConnection(IConnection connection)
        {
            this.connections.Add(connection); return this;
        }
        public FluentMethod WithConnections(List<IConnection> connections)
        {
            return this.WithConnections(connections.ToArray());
        }
        public FluentMethod WithConnections(IConnection[] connections)
        {
            this.connections.AddRange(connections); return this;            
        }
        public T Return<T>()
        {
            if (this.connections.Count == 1)
            {
                return this.context.Return<T>(this.connections[0], this.method, this.arguments, this.genericTypes.ToArray());
            }
            if (this.connections.Count > 1)
            {
                this.Return();
            }
            return default(T);
        }
        public void Return()
        {
            if (this.connections.Count == 1)
            {
                this.context.Return(this.connections[0], this.method, this.arguments, this.genericTypes.ToArray());
            }
            if (this.connections.Count > 1)
            {
                for (int i = 0; i < this.connections.Count; i++)
                {
                    this.context.Return(this.connections[i], this.method, this.arguments, this.genericTypes.ToArray());
                }
            }
        }
        public void Execute()
        {
            this.Return();
        }
        #endregion
    }
}
