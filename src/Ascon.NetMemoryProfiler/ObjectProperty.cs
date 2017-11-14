using Microsoft.Diagnostics.Runtime;

namespace Ascon.NetMemoryProfiler
{
    /// <summary>
    /// Properties to 
    /// </summary>
    public class ObjectProperty
    {
        private const string UNKNOWN = "?";

        private readonly ClrObject _obj;

        internal ObjectProperty(ClrObject obj)
        {
            _obj = obj;
        }

        /// <summary>
        /// Type name of an object
        /// </summary>
        public string Type => _obj.Type?.Name ?? UNKNOWN;

        /// <summary>
        /// Object type namespace
        /// </summary>
        public string Namespace {
            get
            {
                if (_obj.Type == null)
                    return UNKNOWN;
                var index = _obj.Type.Name.LastIndexOf('.');
                if (index == -1)
                    return string.Empty;
                return _obj.Type.Name.Substring(0, index);
            }
        }

        /// <summary>
        /// Object size in bytes
        /// </summary>
        public ulong Size => _obj.Size;
    }
}
