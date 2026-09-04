using System.Windows;
using RMALabs.CharCounter.WPF.ViewModels;

namespace RMALabs.CharCounterWPF
{
    public partial class MainWindow : Window
    {
        public MainWindow(MainViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
