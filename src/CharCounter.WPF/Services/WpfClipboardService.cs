using System.Windows;

namespace RMALabs.CharCounter.WPF.Services
{
    public class WpfClipboardService : IClipboardService
    {
        public void SetText(string text)
        {
            Clipboard.SetText(text);
        }
    }
}
