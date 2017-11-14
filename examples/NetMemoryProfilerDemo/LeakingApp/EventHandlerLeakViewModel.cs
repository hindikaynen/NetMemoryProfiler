using System;
using System.Linq;
using System.Windows.Threading;

namespace LeakingApp
{
    // to fix: use weak events or unsubscribe on close
    public class EventHandlerLeakViewModel : PropertyChangedBase
    {
        public EventHandlerLeakViewModel()
        {
            Title = "EventHandlerLeakViewModel is leaking because it subscribes (and does not unsubscribe) to the event of an object with infinite lifetime";
            Dispatcher.CurrentDispatcher.ShutdownStarted += OnShutdownStarted;
        }

        public string Title { get; set; }

        private void OnShutdownStarted(object sender, EventArgs e)
        {
        }
    }
}
