using System;
using System.Linq;

namespace LeakingApp
{

    // to fix: implement INotifyPropertyChanged
    public class BindingLeakViewModel
    {
        public BindingLeakViewModel()
        {
            Title = "BindingLeakViewModel is leaking because it is used as a binding target and it does not implement INotifyPropertyChanged.";
        }

        public string Title { get; set; }
    }
}
