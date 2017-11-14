using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace LeakingApp
{
    // to fix: use ObservableCollection<object> instead of List<object>
    public class CollectionLeakViewModel : PropertyChangedBase
    {
        public string Title { get; set; }
        public List<object> Items { get; private set; }

        public CollectionLeakViewModel()
        {
            Title = "MyCollectionItems are leaking because it is used as the ItemsSource binding target and does not implement INotifyCollectionChanged";
            Items = new List<object>();
            Items.Add(new MyCollectionItem { Title = "Item 1" });
            Items.Add(new MyCollectionItem { Title = "Item 2" });
        }
    }

    public class MyCollectionItem : PropertyChangedBase
    {
        public string Title { get; set; }
    }
}
