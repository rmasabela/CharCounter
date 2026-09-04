using Xunit;
using RMALabs.CharCounter.WPF;
using RMALabs.CharCounter.WPF.ViewModels;
using RMALabs.CharCounter.WPF.Tests.TestDoubles;

namespace RMALabs.CharCounter.WPF.Tests
{
    public class MainWindowSmokeTests
    {
        [StaFact]
        public void MainWindow_Construction_SetsMainViewModelAsDataContext()
        {
            // Arrange
            var viewModel = new MainViewModel(new FakeTextAnalysisService(), new FakeClipboardService());

            // Act
            var window = new MainWindow(viewModel);

            // Assert
            Assert.Same(viewModel, window.DataContext);
        }
    }
}
