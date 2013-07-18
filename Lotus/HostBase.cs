using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Lotus
{
    public abstract class HostBase<TContainer>
    {
        #region Constructors
        public HostBase()
        {
            this.Host = new Host<TContainer>();
        }
        #endregion

        #region Fluent Fields
        #endregion

        #region Properties
        public Host<TContainer> Host { get; set; }
        #endregion
        
        #region Fluent Methods
        public FluentMethod Method(string method)
        {
            return new FluentMethod(method, this.Host);
        }
        public FluentProperty Property(string property)
        {
            return new FluentProperty(property, this.Host);
        }
        #endregion

        #region Methods
        protected void Initialize(TContainer container)
        {
            this.Host.Initialize(container);
        }
        #endregion
    }
}
