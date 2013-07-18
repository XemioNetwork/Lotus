using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Lotus;
using Lotus.Serialization;

namespace Lotus
{
    public abstract class NetworkObject
    {
        #region Constructors
        public NetworkObject()
        {
            switch (Network.CompressionLevel)
            {
                case CompressionLevel.None:
                    switch (Network.TransferMode)
                    {
                        case TransferMode.Local:
                            this.Serializer = new LocalSerializer();
                            break;
                        case TransferMode.Default:
                        case TransferMode.Tiny:
                            this.Serializer = new TinySerializer();
                            break;
                        case TransferMode.Binary:
                            this.Serializer = new BinarySerializer();
                            break;
                        case TransferMode.Plaintext:
                            this.Serializer = new XmlSerializer();
                            break;
                    }
                    break;
                case CompressionLevel.Compressed:
                    switch (Network.TransferMode)
                    {
                        case TransferMode.Local:
                            this.Serializer = new LocalSerializer();
                            break;
                        case TransferMode.Default:
                        case TransferMode.Tiny:
                            this.Serializer = new CompressedSerializer<TinySerializer>();
                            break;
                        case TransferMode.Binary:
                            this.Serializer = new CompressedSerializer<BinarySerializer>();
                            break;
                        case TransferMode.Plaintext:
                            this.Serializer = new CompressedSerializer<XmlSerializer>();
                            break;
                    }
                    break;
            }

            this.Handlers = new List<DataHandler>();
            this.ReceivedData += new EventHandler<ReceivedDataEventArgs>(OnReceivedData);
        }
        #endregion

        #region Fields
        private object lastObject = null;
        #endregion

        #region Events
        public event EventHandler<ReceivedDataEventArgs> ReceivedData;
        #endregion

        #region Event Handlers
        void OnReceivedData(object sender, ReceivedDataEventArgs e)
        {
            this.lastObject = e.Data;

            IConnection connection = (IConnection)sender;
            Type dataType = e.Data.GetType();

            for (int i = 0; i < this.Handlers.Count; i++)
            {
                if (this.Handlers[i].DataType == dataType ||
                    this.Handlers[i].DataType.IsAssignableFrom(dataType))
                {
                    this.Handlers[i].OnRecieveData(this, connection, e.Data);
                }
            }
        }
        #endregion

        #region Properties
        public ISerializer Serializer { get; set; }
        public List<DataHandler> Handlers { get; set; }

        public abstract IConnection Connection { get; }
        #endregion

        #region Methods
        public virtual void Send(object value)
        {
            this.Send(this.Connection, value);
        }
        public void Send(IConnection connection, object value)
        {
            if (value != null)
            {
                this.Serializer.Serialize(connection.Stream, value);
                connection.Stream.Flush();
            }
            else
            {
                this.SendNull(connection);
            }
        }
        public void SendNull()
        {
            this.Send(new object());
        }
        public void SendNull(IConnection connection)
        {
            this.Send(connection, new object());
        }
        public object ReceiveObject()
        {
            while (this.lastObject == null) ;

            object value = this.lastObject;
            this.lastObject = null;

            return value;
        }
        public T ReceiveObject<T>()
        {
            object obj = this.ReceiveObject();

            if (obj is T)
            {
                return (T)obj;
            }
            else
            {
                return default(T);
            }
        }
        public virtual object WaitObject()
        {
            return this.WaitObject(this.Connection);
        }
        public object WaitObject(IConnection connection)
        {
            return this.Serializer.Deserialize(connection.Stream);
        }
        public virtual T WaitObject<T>()
        {
            return this.WaitObject<T>(this.Connection);
        }
        public T WaitObject<T>(IConnection connection)
        {
            return this.Serializer.Deserialize<T>(connection.Stream);
        }
        public void InvokeReceivedData(object sender, ReceivedDataEventArgs e)
        {
            if (this.ReceivedData != null)
            {
                this.ReceivedData(sender, e);
            }
        }
        #endregion
    }
}
