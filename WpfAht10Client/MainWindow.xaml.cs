using WpfAht10Client.ViewModels;

namespace WpfAht10Client
{
    public partial class MainWindow 
    {
        public MainWindow(MainViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
    }
}