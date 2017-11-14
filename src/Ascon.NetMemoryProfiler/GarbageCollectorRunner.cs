using System;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Reflection;
using System.Threading;
using GarbageCollector;

namespace Ascon.NetMemoryProfiler
{
    static class GarbageCollectorRunner
    {
        private const int CONNECT_TIMEOUT = 100;
        private const int MAX_ATTEMPTS = 10;

        public static void ForceGarbageCollectionInRemoteProcess(int pid)
        {
            if (!CheckIfInjected(pid))
                Inject(pid);

            int attempt = 0;
            bool collected = false;
            while (!collected)
            {
                if(attempt >= MAX_ATTEMPTS)
                    throw new InvalidOperationException("Unable to force GC.Collect on remote process");

                try
                {
                    collected = Request(Const.GC_COLLECT_COMMAND);
                }
                catch(TimeoutException)
                {
                    attempt++;
                    Thread.Sleep(100);
                }
            }
        }

        private static void Inject(int pid)
        {
            var process = Process.GetProcessById(pid);
            var location = Assembly.GetEntryAssembly().Location;
            var directory = Path.GetDirectoryName(location);
            var osArchFolder = Environment.Is64BitOperatingSystem ? "x64" : "x86";
            var file = Path.Combine(directory, "ManagedInjector", osArchFolder, "ManagedInjector.exe");
            var args = $"{process.MainWindowHandle} \"{typeof(GcController).Assembly.Location}\" \"{typeof(GcController).FullName}\" \"Start\"";
            Process.Start(file, args);
        }

        private static bool CheckIfInjected(int pid)
        {
            var process = Process.GetProcessById(pid);
            return process.Modules.OfType<ProcessModule>().Any(x => x.FileName.Contains(Const.INJECTED_ASSEMBLY_NAME));
        }

        private static bool Request(byte command)
        {
            using (var client = new NamedPipeClientStream(".", Const.GC_CONTROLLER_ADDRESS, PipeDirection.InOut))
            {
                client.Connect(CONNECT_TIMEOUT);
                client.WriteByte(command);
                client.WaitForPipeDrain();
                return client.ReadByte() == Const.OK_RESPONSE;
            }
        }
    }
}
