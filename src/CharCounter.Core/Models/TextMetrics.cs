namespace RMALabs.CharCounter.Core.Models
{
    public readonly record struct TextMetrics(
        int TotalChars,
        int CharsNoSpaces,
        int Words,
        int Lines,
        double ReadingMinutes);
}
