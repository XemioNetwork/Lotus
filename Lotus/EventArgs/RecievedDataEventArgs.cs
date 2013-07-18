using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Lotus
{
    public class ReceivedDataEventArgs : EventArgs
    {
        #region Constructors
        public ReceivedDataEventArgs(object data)
        {
            this.Data = data;
        }
        #endregion

        #region Properties
        public object Data { get; set; }
        #endregion
    }
}
