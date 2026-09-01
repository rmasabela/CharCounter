using System;
using System.Buffers;
using System.Windows;
using System.Windows.Controls;

namespace RMALabs.CharCounterWPF
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
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

            ReadOnlySpan<char> span = text.AsSpan();

            int totalChars = span.Length;
            int charsNoSpaces = 0;
            int words = 0;
            int lines = 1;

            bool inWord = false;

            for (int i = 0; i < span.Length; i++)
            {
                char c = span[i];

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

            // Estimate reading time assuming ~200 words per minute
            double readingMinutes = Math.Ceiling((double)words / 200.0);
            if (words == 0) readingMinutes = 0;

            LblTotalChars.Text = totalChars.ToString("N0");
            LblCharsNoSpaces.Text = charsNoSpaces.ToString("N0");
            LblWords.Text = words.ToString("N0");
            LblLines.Text = lines.ToString("N0");
            LblReadingTime.Text = readingMinutes < 1 && words > 0 ? "< 1 min" : $"{readingMinutes} min";
            LblStatus.Text = $"{totalChars:N0} characters analyzed";
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