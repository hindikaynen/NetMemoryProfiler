using System.Collections.Generic;
using System.Linq;
using Microsoft.Diagnostics.Runtime;

namespace Ascon.NetMemoryProfiler
{
    /// <summary>
    /// Clr thread info
    /// </summary>
    public class ThreadInfo
    {
        /// <summary>
        /// OS id for the thread
        /// </summary>
        public uint Id { get; private set; }

        /// <summary>
        /// Managed thread id
        /// </summary>
        public int ManagedThreadId { get; private set; }

        /// <summary>
        /// The app domain the thread is running in
        /// </summary>
        public ulong AppDomain { get; private set; }

        /// <summary>
        /// True if thread is alive in a process, false if was recently terminated
        /// </summary>
        public bool IsAlive { get; private set; }

        /// <summary>
        /// True if is background thread
        /// </summary>
        public bool IsBackground { get; private set; }

        /// <summary>
        /// True if is GC thread
        /// </summary>
        public bool IsGc { get; private set; }

        /// <summary>
        /// Managed thread stack trace
        /// </summary>
        public IEnumerable<string> StackTraceLines { get; private set; }

        /// <summary>
        /// Last thrown exception on the thread
        /// </summary>
        public ExceptionInfo CurrentExceptionInfo { get; private set; }

        private ThreadInfo()
        {

        }

        internal static ThreadInfo ConvertFrom(ClrThread source)
        {
            return new ThreadInfo
            {
                Id = source.OSThreadId,
                ManagedThreadId = source.ManagedThreadId,
                AppDomain = source.AppDomain,
                IsAlive = source.IsAlive,
                IsBackground = source.IsBackground,
                IsGc = source.IsGC,
                CurrentExceptionInfo = ExceptionInfo.ConvertFrom(source.CurrentException),
                StackTraceLines = source.StackTrace.Select(x => x.DisplayString).ToList()
            };
        }
    }

    /// <summary>
    /// Clr exception info
    /// </summary>
    public class ExceptionInfo
    {
        /// <summary>
        /// Address of the exception object
        /// </summary>
        public ulong Address { get; private set; }

        /// <summary>
        /// HRESULT associated with the exception
        /// </summary>
        public int HResult { get; private set; }

        /// <summary>
        /// Inner exception if exists, otherwise null
        /// </summary>
        public ExceptionInfo Inner { get; private set; }

        /// <summary>
        /// Exception message
        /// </summary>
        public string Message { get; private set; }

        /// <summary>
        /// Exception type name
        /// </summary>
        public string Type { get; private set; }

        /// <summary>
        /// Exception stack trace
        /// </summary>
        public IEnumerable<string> StackTraceLines { get; private set; }

        private ExceptionInfo()
        {
        }

        internal static ExceptionInfo ConvertFrom(ClrException source)
        {
            if (source == null)
                return null;

            return new ExceptionInfo
            {
                Address = source.Address,
                HResult = source.HResult,
                Inner = ConvertFrom(source.Inner),
                Message = source.Message,
                Type = source.Type.Name,
                StackTraceLines = source.StackTrace.Select(x=>x.DisplayString).ToList()
            };
        }
    }
}
