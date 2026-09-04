using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RMALabs.CharCounter.Core.Models;
using RMALabs.CharCounter.Core.Services;
using RMALabs.CharCounter.WPF.Services;

namespace RMALabs.CharCounter.WPF.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly ITextAnalysisService _textAnalysisService;
        private readonly IClipboardService _clipboardService;

        public MainViewModel(ITextAnalysisService textAnalysisService, IClipboardService clipboardService)
        {
            _textAnalysisService = textAnalysisService ?? throw new ArgumentNullException(nameof(textAnalysisService));
            _clipboardService = clipboardService ?? throw new ArgumentNullException(nameof(clipboardService));
        }

        [ObservableProperty]
        private string _inputText = string.Empty;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(TotalCharsFormatted))]
        [NotifyPropertyChangedFor(nameof(CharsNoSpacesFormatted))]
        [NotifyPropertyChangedFor(nameof(WordsFormatted))]
        [NotifyPropertyChangedFor(nameof(LinesFormatted))]
        [NotifyPropertyChangedFor(nameof(ReadingTimeFormatted))]
        private TextMetrics _metrics = new(0, 0, 0, 0, 0.0);

        [ObservableProperty]
        private string _statusText = "Ready";

        public string TotalCharsFormatted => Metrics.TotalChars.ToString("N0");

        public string CharsNoSpacesFormatted => Metrics.CharsNoSpaces.ToString("N0");

        public string WordsFormatted => Metrics.Words.ToString("N0");

        public string LinesFormatted => Metrics.Lines.ToString("N0");

        public string ReadingTimeFormatted => Metrics.ReadingMinutes < 1 && Metrics.Words > 0 
            ? "< 1 min" 
            : $"{Metrics.ReadingMinutes} min";

        partial void OnInputTextChanged(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                Metrics = new TextMetrics(0, 0, 0, 0, 0.0);
                StatusText = "Ready";
                return;
            }

            Metrics = _textAnalysisService.Analyze(value.AsSpan());
            StatusText = $"{Metrics.TotalChars:N0} characters analyzed";
        }

        [RelayCommand]
        private void ClearText()
        {
            InputText = string.Empty;
            StatusText = "Text cleared";
        }

        [RelayCommand]
        private void CopyText()
        {
            if (!string.IsNullOrEmpty(InputText))
            {
                _clipboardService.SetText(InputText);
                StatusText = "Copied to clipboard";
            }
        }
    }
}
