using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.Diagnostics.Runtime;

namespace Ascon.NetMemoryProfiler
{
    /// <summary>
    /// Set of APIs for memory analysis of .NET processes
    /// </summary>
    public static class Profiler
    {
        /// <summary>
        /// Attaches profiler to the process
        /// </summary>
        /// <param name="processName">Process name</param>
        /// <param name="timeout">Timeout in ms</param>
        /// <returns>Profiling session</returns>
        public static IProfilerSession AttachToProcess(string processName, uint timeout = 5000)
        {
            var processes = Process.GetProcessesByName(processName);
            if (!processes.Any())
                throw new InvalidOperationException($"Process {processName} not found");
            if (processes.Length > 1)
                throw new InvalidOperationException($"Multiple processes {processName} found. Use the Profiler.AttachToProcess(int pid) overload to determine which process you want to attach.");

            return AttachToProcess(processes.First().Id);
        }

        /// <summary>
        /// Attaches profiler to the process
        /// </summary>
        /// <param name="pid">Process ID</param>
        /// <param name="timeout">Timeout in ms</param>
        /// <returns>Profiling session</returns>
        public static IProfilerSession AttachToProcess(int pid, uint timeout = 5000)
        {
            GarbageCollectorRunner.ForceGarbageCollectionInRemoteProcess(pid);
            return new ProfilerSession(DataTarget.AttachToProcess(pid, timeout));
        }

        /// <summary>
        /// Creates the profiler session from memory dump
        /// </summary>
        /// <param name="filePath">Path to the memory dump</param>
        /// <returns>Profiling session</returns>
        public static IProfilerSession LoadMemoryDump(string filePath)
        {
            return new ProfilerSession(DataTarget.LoadCrashDump(filePath));
        }
    }
}