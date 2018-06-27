using System;
using System.Collections.Generic;
// ReSharper disable InheritdocConsiderUsage

namespace Ascon.NetMemoryProfiler
{
    /// <summary>
    /// Profiling session
    /// </summary>
    public interface IProfilerSession : IDisposable
    {
        /// <summary>
        /// Finds objects that matches specified criteria
        /// </summary>
        /// <param name="where">The ObjectProperty object passed to the lambda allows creating queries that select objects by type, namespace or size. </param>
        IEnumerable<InstanceInfo> GetAliveObjects(Predicate<ObjectProperty> where);

        /// <summary>
        /// Finds paths from GC roots which prevent the instances from being collected. 
        /// Note that executing this method could take a long time and it is recommended to call it once for all the interested instances.
        /// </summary>
        /// <param name="instances">Instances to analyze</param>
        IEnumerable<RetentionsInfo> FindRetentions(IEnumerable<InstanceInfo> instances);

        /// <summary>
        /// Returns true if the GC heap is in a consistent state for heap enumeration.  This will return false
        /// if the process was stopped in the middle of a GC, which can cause the GC heap to be unwalkable.
        /// </summary>
        bool IsHeapConsistent { get; }

        /// <summary>
        /// Returns current threads of the process.
        /// </summary>
        /// <returns></returns>
        IEnumerable<ThreadInfo> GetCurrentThreads();
    }
}