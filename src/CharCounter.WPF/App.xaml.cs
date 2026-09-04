using Microsoft.Extensions.DependencyInjection;
using RMALabs.CharCounter.Core.Services;
using RMALabs.CharCounter.WPF.Services;
using RMALabs.CharCounter.WPF.ViewModels;
using System.Configuration;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Windows;

namespace RMALabs.CharCounterWPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    [ExcludeFromCodeCoverage]
    public partial class App : Application
    {
        private ServiceProvider? _serviceProvider;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var services = new ServiceCollection();
            services.AddSingleton<ITextAnalysisService, TextAnalysisService>();
            services.AddSingleton<IClipboardService, WpfClipboardService>();
            services.AddTransient<MainViewModel>();
            services.AddTransient<MainWindow>();

            _serviceProvider = services.BuildServiceProvider();

            var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _serviceProvider?.Dispose();
            base.OnExit(e);
        }
    }

}
