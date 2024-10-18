
using System.Windows;
using WpfAht10Client.ViewModels;

namespace WpfAht10Client
{
    public partial class MainWindow : Window
    {
        public MainWindow(MainViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
    }
}