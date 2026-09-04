using System.Diagnostics.CodeAnalysis;
using System.Windows;

namespace RMALabs.CharCounter.WPF.Services
{
    [ExcludeFromCodeCoverage]
    public class WpfClipboardService : IClipboardService
    {
        public void SetText(string text)
        {
            Clipboard.SetText(text);
        }
    }
}
