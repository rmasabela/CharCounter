using System;
using RMALabs.CharCounter.Core.Models;

namespace RMALabs.CharCounter.Core.Services
{
    public interface ITextAnalysisService
    {
        TextMetrics Analyze(ReadOnlySpan<char> text);
    }
}
