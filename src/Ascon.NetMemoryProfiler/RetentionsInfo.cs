using System.Collections.Generic;

namespace Ascon.NetMemoryProfiler
{
    /// <summary>
    /// </summary>
    public class RetentionsInfo
    {
        /// <summary>
        /// Instance
        /// </summary>
        public InstanceInfo Instance { get; }

        /// <summary>
        /// Paths from GC roots which prevent the instance from being collected
        /// </summary>
        public List<string> RetentionPaths { get; }

        internal RetentionsInfo(InstanceInfo instance)
        {
            Instance = instance;
            RetentionPaths = new List<string>();
        }
    }
}
