using System.Collections.Generic;
using System.ComponentModel;
using Xunit;
using RMALabs.CharCounter.Core.Models;
using RMALabs.CharCounter.WPF.ViewModels;
using RMALabs.CharCounter.WPF.Tests.TestDoubles;

namespace RMALabs.CharCounter.WPF.Tests
{
    public class MainViewModelTests
    {
        private readonly FakeTextAnalysisService _textAnalysisService;
        private readonly FakeClipboardService _clipboardService;
        private readonly MainViewModel _viewModel;

        public MainViewModelTests()
        {
            _textAnalysisService = new FakeTextAnalysisService();
            _clipboardService = new FakeClipboardService();
            _viewModel = new MainViewModel(_textAnalysisService, _clipboardService);
        }

        [Fact]
        public void Constructor_NullTextAnalysisService_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new MainViewModel(null!, _clipboardService));
        }

        [Fact]
        public void Constructor_NullClipboardService_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new MainViewModel(_textAnalysisService, null!));
        }

        [Fact]
        public void Constructor_ValidServices_InitializesWithDefaultState()
        {
            Assert.Equal(string.Empty, _viewModel.InputText);
            Assert.Equal(new TextMetrics(0, 0, 0, 0, 0.0), _viewModel.Metrics);
            Assert.Equal("Ready", _viewModel.StatusText);
        }

        [Fact]
        public void OnInputTextChanged_NonEmptyText_UpdatesMetricsAndStatus()
        {
            // Arrange
            _textAnalysisService.MetricsToReturn = new TextMetrics(10, 9, 2, 1, 1.0);

            // Act
            _viewModel.InputText = "Hola mundo";

            // Assert
            Assert.Equal(new TextMetrics(10, 9, 2, 1, 1.0), _viewModel.Metrics);
            Assert.Equal("10 characters analyzed", _viewModel.StatusText);
            Assert.Equal("Hola mundo", _textAnalysisService.LastAnalyzedText);
        }

        [Fact]
        public void OnInputTextChanged_EmptyText_ResetsMetricsAndStatus()
        {
            // Arrange
            _textAnalysisService.MetricsToReturn = new TextMetrics(10, 9, 2, 1, 1.0);
            _viewModel.InputText = "Hola mundo";

            // Act
            _viewModel.InputText = string.Empty;

            // Assert
            Assert.Equal(new TextMetrics(0, 0, 0, 0, 0.0), _viewModel.Metrics);
            Assert.Equal("Ready", _viewModel.StatusText);
        }

        [Theory]
        [InlineData(1234, "1,234")]
        [InlineData(999, "999")]
        [InlineData(1000000, "1,000,000")]
        public void FormattedProperties_UseThousandsSeparator(int value, string expected)
        {
            // Arrange
            _textAnalysisService.MetricsToReturn = new TextMetrics(value, value, value, value, 1.0);

            // Act
            _viewModel.InputText = "trigger";

            // Assert
            Assert.Equal(expected, _viewModel.TotalCharsFormatted);
            Assert.Equal(expected, _viewModel.CharsNoSpacesFormatted);
            Assert.Equal(expected, _viewModel.WordsFormatted);
            Assert.Equal(expected, _viewModel.LinesFormatted);
        }

        [Fact]
        public void ReadingTimeFormatted_WordsZero_ReturnsZeroMinutes()
        {
            // Arrange
            _textAnalysisService.MetricsToReturn = new TextMetrics(3, 0, 0, 1, 0.0);

            // Act
            _viewModel.InputText = "   ";

            // Assert
            Assert.Equal("0 min", _viewModel.ReadingTimeFormatted);
        }

        [Fact]
        public void ReadingTimeFormatted_ReadingMinutesLessThanOneAndWordsPositive_ReturnsLessThanOneMinute()
        {
            // Arrange
            _textAnalysisService.MetricsToReturn = new TextMetrics(5, 5, 1, 1, 0.0);

            // Act
            _viewModel.InputText = "Hola";

            // Assert
            Assert.Equal("< 1 min", _viewModel.ReadingTimeFormatted);
        }

        [Fact]
        public void ReadingTimeFormatted_ReadingMinutesOneOrMore_ReturnsFormattedMinutes()
        {
            // Arrange
            _textAnalysisService.MetricsToReturn = new TextMetrics(50, 45, 10, 1, 2.0);

            // Act
            _viewModel.InputText = "Hola mundo";

            // Assert
            Assert.Equal("2 min", _viewModel.ReadingTimeFormatted);
        }

        [Fact]
        public void MetricsChanged_RaisesPropertyChangedForAllFormattedProperties()
        {
            // Arrange
            var raisedProperties = new List<string>();
            _viewModel.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName is not null)
                {
                    raisedProperties.Add(args.PropertyName);
                }
            };
            _textAnalysisService.MetricsToReturn = new TextMetrics(10, 9, 2, 1, 1.0);

            // Act
            _viewModel.InputText = "Hola mundo";

            // Assert
            Assert.Contains(nameof(MainViewModel.TotalCharsFormatted), raisedProperties);
            Assert.Contains(nameof(MainViewModel.CharsNoSpacesFormatted), raisedProperties);
            Assert.Contains(nameof(MainViewModel.WordsFormatted), raisedProperties);
            Assert.Contains(nameof(MainViewModel.LinesFormatted), raisedProperties);
            Assert.Contains(nameof(MainViewModel.ReadingTimeFormatted), raisedProperties);
        }

        [Fact]
        public void ClearTextCommand_ResetsInputTextAndSetsStatusToTextCleared()
        {
            // Arrange
            _textAnalysisService.MetricsToReturn = new TextMetrics(10, 9, 2, 1, 1.0);
            _viewModel.InputText = "Hola mundo";

            // Act
            _viewModel.ClearTextCommand.Execute(null);

            // Assert
            Assert.Equal(string.Empty, _viewModel.InputText);
            Assert.Equal(new TextMetrics(0, 0, 0, 0, 0.0), _viewModel.Metrics);
            Assert.Equal("Text cleared", _viewModel.StatusText);
        }

        [Fact]
        public void CopyTextCommand_NonEmptyInputText_CallsClipboardServiceAndUpdatesStatus()
        {
            // Arrange
            _viewModel.InputText = "Hola mundo";

            // Act
            _viewModel.CopyTextCommand.Execute(null);

            // Assert
            Assert.Equal("Hola mundo", _clipboardService.LastText);
            Assert.Equal("Copied to clipboard", _viewModel.StatusText);
        }

        [Fact]
        public void CopyTextCommand_EmptyInputText_DoesNothing()
        {
            // Act
            _viewModel.CopyTextCommand.Execute(null);

            // Assert
            Assert.Null(_clipboardService.LastText);
            Assert.Equal("Ready", _viewModel.StatusText);
        }
    }
}
