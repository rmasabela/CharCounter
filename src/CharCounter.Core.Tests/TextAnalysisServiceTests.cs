using Xunit;
using RMALabs.CharCounter.Core.Services;
using System;
using RMALabs.CharCounter.Core.Tests.Utilities;

namespace RMALabs.CharCounter.Core.Tests
{
    public class TextAnalysisServiceTests
    {
        private readonly ITextAnalysisService _textAnalysisService;

        public TextAnalysisServiceTests()
        {
            _textAnalysisService = new TextAnalysisService();
        }

        [Fact]
        public void Analyze_EmptySpan_ReturnsZeroMetrics()
        {
            // Arrange
            var emptySpan = ReadOnlySpan<char>.Empty;

            // Act
            var metrics = _textAnalysisService.Analyze(emptySpan);

            // Assert
            Assert.Equal(0, metrics.TotalChars);
            Assert.Equal(0, metrics.CharsNoSpaces);
            Assert.Equal(0, metrics.Words);
            Assert.Equal(0, metrics.Lines);
            Assert.Equal(0, metrics.ReadingMinutes);
        }

        [Theory]
        [InlineData("   ", 3, 1)]
        [InlineData("\t\t", 2, 1)]
        [InlineData(" \n \n ", 5, 3)]
        public void Analyze_WhitespaceOnly_CountsCharactersButNoWords(string input, int expectedTotalChars, int expectedLines)
        {
            // Arrange
            var span = input.AsSpan();

            // Act
            var metrics = _textAnalysisService.Analyze(span);

            // Assert
            Assert.Equal(expectedTotalChars, metrics.TotalChars);
            Assert.Equal(0, metrics.CharsNoSpaces);
            Assert.Equal(0, metrics.Words);
            Assert.Equal(expectedLines, metrics.Lines);
            Assert.Equal(0, metrics.ReadingMinutes);
        }

        [Theory]
        [InlineData("Hola mundo", 10, 9, 2, 1, 1.0)]
        [InlineData("Hola   mundo", 12, 9, 2, 1, 1.0)]
        [InlineData("Hola\tmundo", 10, 9, 2, 1, 1.0)]
        [InlineData("Hola\nmundo", 10, 9, 2, 2, 1.0)]
        [InlineData("Hola\r\nmundo", 11, 9, 2, 2, 1.0)]
        [InlineData("¡Programación con C# 10 y WPF!", 30, 25, 6, 1, 2.0)]
        public void Analyze_RepresentativeCases_ReturnsExpectedMetrics(
            string input,
            int expectedTotalChars,
            int expectedCharsNoSpaces,
            int expectedWords,
            int expectedLines,
            double expectedReadingMinutes)
        {
            // Arrange
            var span = input.AsSpan();

            // Act
            var metrics = _textAnalysisService.Analyze(span);

            // Assert
            Assert.Equal(expectedTotalChars, metrics.TotalChars);
            Assert.Equal(expectedCharsNoSpaces, metrics.CharsNoSpaces);
            Assert.Equal(expectedWords, metrics.Words);
            Assert.Equal(expectedLines, metrics.Lines);
            Assert.Equal(expectedReadingMinutes, metrics.ReadingMinutes);
        }

        [Fact]
        public void Analyze_CharsNoSpaces_ExcludesAllWhitespaceCharacters()
        {
            // Arrange
            var span = "a \t b\nc\r\nd".AsSpan();

            // Act
            var metrics = _textAnalysisService.Analyze(span);

            // Assert
            Assert.Equal(10, metrics.TotalChars);
            Assert.Equal(4, metrics.CharsNoSpaces);
            Assert.Equal(4, metrics.Words);
            Assert.Equal(3, metrics.Lines);
            Assert.Equal(1.0, metrics.ReadingMinutes);
        }

        [Fact]
        public void Analyze_TrailingWhitespace_DoesNotAddExtraWords()
        {
            // Arrange
            var span = "uno dos   ".AsSpan();

            // Act
            var metrics = _textAnalysisService.Analyze(span);

            // Assert
            Assert.Equal(10, metrics.TotalChars);
            Assert.Equal(6, metrics.CharsNoSpaces);
            Assert.Equal(2, metrics.Words);
            Assert.Equal(1, metrics.Lines);
            Assert.Equal(1.0, metrics.ReadingMinutes);
        }

        [Fact]
        public void Analyze_BlankLines_AreIncludedInLineCount()
        {
            // Arrange
            var span = "uno\n\ndos\n".AsSpan();

            // Act
            var metrics = _textAnalysisService.Analyze(span);

            // Assert
            Assert.Equal(9, metrics.TotalChars);
            Assert.Equal(6, metrics.CharsNoSpaces);
            Assert.Equal(2, metrics.Words);
            Assert.Equal(4, metrics.Lines);
            Assert.Equal(1.0, metrics.ReadingMinutes);
        }

        [Fact]
        public void Analyze_ReadingTime_RoundsUpAppropriately()
        {
            // Arrange
            // Obtenemos el valor de WordsPerMinute desde la clase TextAnalysisService
            var wordsPerMinute = TextAnalysisService.WordsPerMinute;
            var shortText = TextGenerator.GenerateTextWithWords(wordsPerMinute); // 200 palabras (cada palabra tiene 5 caracteres)
            var longText = TextGenerator.GenerateTextWithWords((wordsPerMinute + 1)); // 201 palabras (cada palabra tiene 5 caracteres)

            var shortSpan = shortText.AsSpan();
            var longSpan = longText.AsSpan();

            // Act
            var shortMetrics = _textAnalysisService.Analyze(shortSpan);
            var longMetrics = _textAnalysisService.Analyze(longSpan);

            // Assert
            Assert.Equal(1.0, shortMetrics.ReadingMinutes);
            Assert.Equal(2.0, longMetrics.ReadingMinutes);
        }
    }
}