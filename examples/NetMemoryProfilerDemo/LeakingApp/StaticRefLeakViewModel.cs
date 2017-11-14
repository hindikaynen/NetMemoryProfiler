using System;
using System.Collections.Generic;
using System.Linq;

namespace LeakingApp
{
    public class StaticRefLeakViewModel : PropertyChangedBase
    {
        public StaticRefLeakViewModel()
        {
            Title = "StaticRefLeakViewModel is leaking because it is referenced by the static Foo.Objects property";
            Foo.Objects.Add(this);
        }

        public string Title { get; set; }
    }

    public static class Foo
    {
        static Foo()
        {
            Objects = new List<object>();
        }

        public static List<object> Objects { get; }
    }
}
