using System;
using RMALabs.CharCounter.Core.Models;

namespace RMALabs.CharCounter.Core.Services
{
    public class TextAnalysisService : ITextAnalysisService
    {
        public const int WordsPerMinute = 5;

        public TextMetrics Analyze(ReadOnlySpan<char> text)
        {
            if (text.IsEmpty)
            {
                return new TextMetrics(0, 0, 0, 0, 0.0);
            }

            int totalChars = text.Length;
            int charsNoSpaces = 0;
            int words = 0;
            int lines = 1;

            bool inWord = false;

            for (int i = 0; i < text.Length; i++)
            {
                char c = text[i];

                if (!char.IsWhiteSpace(c))
                {
                    charsNoSpaces++;
                }

                if (c == '\n')
                {
                    lines++;
                }

                if (char.IsWhiteSpace(c))
                {
                    if (inWord)
                    {
                        words++;
                        inWord = false;
                    }
                }
                else
                {
                    inWord = true;
                }
            }

            if (inWord)
            {
                words++;
            }

            double readingMinutes = Math.Ceiling((double)words / WordsPerMinute);
            if (words == 0)
            {
                readingMinutes = 0;
            }

            return new TextMetrics(totalChars, charsNoSpaces, words, lines, readingMinutes);
        }
    }
}
