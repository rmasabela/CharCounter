using System;
using System.Windows;
using System.Windows.Controls;
using RMALabs.CharCounter.Core.Models;
using RMALabs.CharCounter.Core.Services;

namespace RMALabs.CharCounterWPF
{
    public partial class MainWindow : Window
    {
        private readonly ITextAnalysisService _textAnalysisService;

        public MainWindow()
        {
            InitializeComponent();
            _textAnalysisService = new TextAnalysisService();
        }

        private void TxtInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateMetrics(TxtInput.Text);
        }

        private void UpdateMetrics(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                LblTotalChars.Text = "0";
                LblCharsNoSpaces.Text = "0";
                LblWords.Text = "0";
                LblLines.Text = "0";
                LblReadingTime.Text = "0 min";
                LblStatus.Text = "Ready";
                return;
            }

            TextMetrics metrics = _textAnalysisService.Analyze(text.AsSpan());

            LblTotalChars.Text = metrics.TotalChars.ToString("N0");
            LblCharsNoSpaces.Text = metrics.CharsNoSpaces.ToString("N0");
            LblWords.Text = metrics.Words.ToString("N0");
            LblLines.Text = metrics.Lines.ToString("N0");
            LblReadingTime.Text = metrics.ReadingMinutes < 1 && metrics.Words > 0 ? "< 1 min" : $"{metrics.ReadingMinutes} min";
            LblStatus.Text = $"{metrics.TotalChars:N0} characters analyzed";
        }

        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            TxtInput.Clear();
            TxtInput.Focus();
            LblStatus.Text = "Text cleared";
        }

        private void BtnCopy_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(TxtInput.Text))
            {
                Clipboard.SetText(TxtInput.Text);
                LblStatus.Text = "Copied to clipboard";
            }
        }
    }
}
