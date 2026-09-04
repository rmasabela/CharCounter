using RMALabs.CharCounter.WPF.Services;

namespace RMALabs.CharCounter.WPF.Tests.TestDoubles
{
    public class FakeClipboardService : IClipboardService
    {
        public string? LastText { get; private set; }

        public void SetText(string text)
        {
            LastText = text;
        }
    }
}
