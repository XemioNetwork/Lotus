using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Lotus
{
    public abstract class ClientBase<TContainer>
    {
        #region Constructors
        public ClientBase()
        {
            this.Client = new Client<TContainer>();
        }
        #endregion

        #region Fields
        private static object lockObject = new object();
        #endregion

        #region Properties
        public Client<TContainer> Client { get; set; }
        #endregion

        #region Fluent Fields
        private string method = string.Empty;
        private object[] arguments = new object[] { };
        private List<Type> genericTypes = new List<Type>();
        #endregion

        #region Fluent Methods
        public FluentMethod Method(string method)
        {
            return new FluentMethod(method, this.Client, this.Client.Connection);
        }
        public FluentProperty Property(string property)
        {
            return new FluentProperty(property, this.Client);
        }
        #endregion

        #region Methods
        protected void Initialize(TContainer container)
        {
            this.Client.Initialize(container);
        }
        #endregion
    }
}
