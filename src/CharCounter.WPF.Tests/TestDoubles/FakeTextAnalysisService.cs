using RMALabs.CharCounter.Core.Models;
using RMALabs.CharCounter.Core.Services;

namespace RMALabs.CharCounter.WPF.Tests.TestDoubles
{
    public class FakeTextAnalysisService : ITextAnalysisService
    {
        public TextMetrics MetricsToReturn { get; set; } = new(0, 0, 0, 0, 0.0);

        public string? LastAnalyzedText { get; private set; }

        public TextMetrics Analyze(ReadOnlySpan<char> text)
        {
            LastAnalyzedText = text.ToString();
            return MetricsToReturn;
        }
    }
}
