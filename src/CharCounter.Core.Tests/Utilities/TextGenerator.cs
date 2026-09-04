namespace RMALabs.CharCounter.Core.Tests.Utilities
{
    public static class TextGenerator
    {
        public static string GenerateTextWithWords(int numWords)
        {
            string result = string.Empty;
            if (numWords <= 0)
            {
                return result;
            }

            var word = "aaaaa ";

            for (int i = 0; i < numWords; i++)
            {
                result = string.Concat(result, word);
            }

            return result;
        }
    }
}