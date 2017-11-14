using System.Collections.Generic;

namespace Ascon.NetMemoryProfiler
{
    /// <summary>
    /// </summary>
    public class InstanceInfo
    {
        internal ulong Address { get; }
        
        /// <summary>
        /// Object type name
        /// </summary>
        public string TypeName { get; }

        /// <summary>
        /// Object size in bytes
        /// </summary>
        public ulong Size { get; }

        internal InstanceInfo(ulong address, string typeName, ulong size)
        {
            Address = address;
            TypeName = typeName;
            Size = size;
        }
    }
}
