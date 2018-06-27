using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Diagnostics.Runtime;

namespace Ascon.NetMemoryProfiler
{
    class ProfilerSession : IProfilerSession
    {
        private readonly DataTarget _dataTarget;
        private readonly ClrRuntime _runtime;

        internal ProfilerSession(DataTarget dataTarget)
        {
            if (dataTarget == null)
                throw new ArgumentNullException(nameof(dataTarget));
            if (!dataTarget.ClrVersions.Any())
                throw new NotSupportedException("Only .NET process profiling is supported");

            _runtime = dataTarget.ClrVersions.First().CreateRuntime();
            _dataTarget = dataTarget;
        }

        public IEnumerable<InstanceInfo> GetAliveObjects(Predicate<ObjectProperty> where)
        {
            var result = new List<InstanceInfo>();
            foreach (var obj in _runtime.Heap.EnumerateObjects())
            {
                if (where(new ObjectProperty(obj)))
                {
                    result.Add(new InstanceInfo(obj.Address, obj.Type.Name, obj.Size));
                }
            }
            return result;
        }

        public IEnumerable<RetentionsInfo> FindRetentions(IEnumerable<InstanceInfo> instances)
        {
            var result = instances.ToDictionary(x => x.Address, x => new RetentionsInfo(x));
            var walker = new HeapWalker(_runtime.Heap);
            walker.Traverse((addr, getRetentionPath) =>
            {
                RetentionsInfo info;
                if (result.TryGetValue(addr, out info))
                {
                    var retention = getRetentionPath();
                    CommonRetentionsFinder.AddOrReplace(info.RetentionPaths, retention);
                    return true;
                }
                return false;
            });
            return result.Values.ToList();
        }

        public bool IsHeapConsistent => _runtime.Heap.CanWalkHeap;

        public IEnumerable<ThreadInfo> GetCurrentThreads()
        {
            return _runtime.Threads.Select(ThreadInfo.ConvertFrom).ToList();
        }

        public void Dispose()
        {
            _dataTarget.Dispose();
        }
    }
}
