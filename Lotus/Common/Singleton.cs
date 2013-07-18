using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lotus
{
    public class Singleton<T> where T : class, new()
    {
        #region Static Fields
        private static Lazy<T> instance = new Lazy<T>(() => new T());
        #endregion

        #region Static Properties
        public static T Instance
        {
            get { return instance.Value; }
        }
        #endregion
    }
}
