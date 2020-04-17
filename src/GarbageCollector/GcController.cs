using System;
using System.IO.Pipes;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Threading;

namespace GarbageCollector
{
    public class GcController : IDisposable
    {
        private readonly string _address;
        private NamedPipeServerStream _pipeServer;
        private readonly object _locker = new object();
        private bool _isStopped;

        public static GcController Start()
        {
            return new GcController(Const.GC_CONTROLLER_ADDRESS);
        }

        public GcController(string address)
        {
            _address = address;

            new Thread(Processing)
            {
                IsBackground = true
            }.Start();
        }

        private void Processing()
        {
            while (true)
            {
                try
                {
                    PipeSecurity ps = new PipeSecurity();
                    var rule = new PipeAccessRule(
                        new SecurityIdentifier(WellKnownSidType.AuthenticatedUserSid, null),
                        PipeAccessRights.ReadWrite,
                        AccessControlType.Allow);

                    ps.SetAccessRule(rule);
                    using (_pipeServer = new NamedPipeServerStream(_address, PipeDirection.InOut, 1, PipeTransmissionMode.Byte, PipeOptions.None, 0, 0, ps))
                    {
                        while (true)
                        {
                            _pipeServer.WaitForConnection();
                            lock (_locker)
                            {
                                if (_isStopped)
                                    return;
                            }
                            var request = _pipeServer.ReadByte();

                            if (request == Const.GC_COLLECT_COMMAND)
                            {
                                GC.Collect();
                                GC.WaitForPendingFinalizers();
                                GC.Collect();

                                _pipeServer.WriteByte(Const.OK_RESPONSE);
                            }
                            _pipeServer.Disconnect();
                        }
                    }
                }
                catch 
                {
                    Thread.Sleep(10);
                }
            }
        }

        public void Dispose()
        {
            ConnectToShutdown();
        }

        private void ConnectToShutdown()
        {
            using (var client = new NamedPipeClientStream(".", _address, PipeDirection.Out))
            {
                lock (_locker)
                {
                    client.Connect();
                    _isStopped = true;
                }
            }
        }
    }
}
