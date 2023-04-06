using CryptoCurrencyHelper.ViewModel;
using System.Windows;

namespace CryptoCurrencyHelper
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
            MainViewModel = new MyViewModel();
            DataContext = MainViewModel;
        }

        public MyViewModel MainViewModel { get; }
    }
}
