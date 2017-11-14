using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LeakingApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void BindingLeakButtonClick(object sender, RoutedEventArgs e)
        {
            var view = new BindingLeakView();
            view.DataContext = new BindingLeakViewModel();
            view.ShowDialog();
        }

        private void EventHandlerLeakButtonClick(object sender, RoutedEventArgs e)
        {
            var view = new EventHandlerLeakView();
            view.DataContext = new EventHandlerLeakViewModel();
            view.ShowDialog();
        }

        private void StaticRefLeakButtonClick(object sender, RoutedEventArgs e)
        {
            var view = new StaticRefLeakView();
            view.DataContext = new StaticRefLeakViewModel();
            view.ShowDialog();
        }

        private void CollectionLeakButtonClick(object sender, RoutedEventArgs e)
        {
            var view = new CollectionLeakView();
            view.DataContext = new CollectionLeakViewModel();
            view.ShowDialog();
        }
    }
}
