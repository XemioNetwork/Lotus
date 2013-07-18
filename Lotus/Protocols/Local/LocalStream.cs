using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Threading;
using System.Reflection;

namespace Lotus
{
    public class LocalStream : MemoryStream
    {
        #region Singleton

        #region Static Fields
        private static Lazy<LocalStream> instance = new Lazy<LocalStream>(() => new LocalStream());
        #endregion

        #region Static Properties
        public static LocalStream Instance
        {
            get { return instance.Value; }
        }
        #endregion

        #endregion
        
        #region Properties
        public NetworkClient BaseClient { get; set; }
        public NetworkHost BaseHost { get; set; }

        public object ClientContainer { get; set; }
        public object HostContainer { get; set; }
        #endregion

        #region Methods
        public void ExecuteAtClient(NetworkMethod info)
        {
            this.ExecuteMethod(info, this.ClientContainer);
        }
        public void ExecuteAtServer(NetworkMethod info)
        {
            this.ExecuteMethod(info, this.HostContainer);
        }
        public void ExecuteMethod(NetworkMethod info)
        {
            switch (info.Direction)
            {
                case NetworkMethodDirection.ClientToServer:
                    this.ExecuteAtServer(info);
                    break;
                case NetworkMethodDirection.ServerToClient:
                    this.ExecuteAtClient(info);
                    break;
            }
        }
        private void ExecuteMethod(NetworkMethod info, object target)
        {
            object result = null;
            Type type = target.GetType();

            switch (info.Direction)
            {
                case NetworkMethodDirection.ClientToServer:
                    result = (this.BaseHost as IExecutionContext).GetMethod(info.Method).Invoke(target, info.Arguments);
                    if (result != null)
                    {
                        this.BaseClient.InvokeReceivedData(LocalConnection.Instance, new ReceivedDataEventArgs(result));
                    }
                    break;
                case NetworkMethodDirection.ServerToClient:
                    result = (this.BaseClient as IExecutionContext).GetMethod(info.Method).Invoke(target, info.Arguments);
                    if (result != null)
                    {
                        this.BaseHost.InvokeReceivedData(LocalConnection.Instance, new ReceivedDataEventArgs(result));
                    }
                    break;
            }
        }
        #endregion

        #region MemoryStream Member
        public override int ReadByte() { return 0; }
        public override int Read(byte[] buffer, int offset, int count) { return 0; }
        public override void WriteByte(byte value) { }
        public override void Write(byte[] buffer, int offset, int count) { }
        #endregion
    }
}
