using System.Windows;
using RMALabs.CharCounter.Core.Services;
using RMALabs.CharCounter.WPF.ViewModels;

namespace RMALabs.CharCounterWPF
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel(new TextAnalysisService());
        }
    }
}
