using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Lotus;

namespace Lotus
{
    public enum TransferMode
    {
        /// <summary>
        /// Userspecific serializer.
        /// </summary>
        Undefined,

        /// <summary>
        /// Optimized configuration for local purposes.
        /// </summary>
        Local,

        /// <summary>
        /// Extended binary mode using tiny serialization.
        /// </summary>
        Tiny,

        /// <summary>
        /// Default binary mode.
        /// </summary>
        Binary,

        /// <summary>
        /// Plaintext data using XML.
        /// </summary>
        Plaintext,

        /// <summary>
        /// Compressed optimized binary mode using tiny serialization and GZip.
        /// </summary>
        Default
    }
    public enum Protocol
    {
        /// <summary>
        /// Highspeed data transfer on a local machine.
        /// </summary>
        Direct,

        /// <summary>
        /// Safe network data transfer.
        /// </summary>
        TCP
    }
    public enum CompressionLevel
    {
        /// <summary>
        /// Compress network data.
        /// </summary>
        Compressed,

        /// <summary>
        /// Send uncompressed network data.
        /// </summary>
        None
    }
    public enum SecurityMode
    {
        /// <summary>
        /// Enable server sided permissions.
        /// </summary>
        Default,

        /// <summary>
        /// Disable security systems.
        /// </summary>
        None
    }
    public class Network
    {
        #region Constructors
        static Network()
        {
            Network.Protocol = Protocol.TCP;
            Network.TransferMode = TransferMode.Default;
            Network.CompressionLevel = CompressionLevel.Compressed;
            Network.SecurityMode = SecurityMode.Default;
        }
        #endregion

        #region Properties
        public static Protocol Protocol { get; set; }
        public static TransferMode TransferMode { get; set; }
        public static CompressionLevel CompressionLevel { get; set; }
        public static SecurityMode SecurityMode { get; set; }
        #endregion
    }
}
