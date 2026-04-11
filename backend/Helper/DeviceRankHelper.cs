using backend.Entity;
using System.Text;
using System.Text.RegularExpressions;

namespace backend.Helper
{
    public static class DeviceRankHelper
    {
        public static int CalculateScore(Device device, string normalizedQuery, List<string> queryTokens)
        {
            var normalizedName = NormalizeInput(device.Name ?? string.Empty);
            var normalizedManufacturer = NormalizeInput(device.Manufacturer ?? string.Empty);
            var normalizedProcessor = NormalizeInput(device.Processor ?? string.Empty);
            var normalizedRam = NormalizeRam(device.RamAmount);

            var combined = $"{normalizedName} {normalizedManufacturer} {normalizedProcessor} {normalizedRam}".Trim();

            var fields = new List<(string value, int exact, int partial)>
                {
                    (normalizedName, 10, 5),
                    (normalizedManufacturer, 8, 4),
                    (normalizedRam, 6, 3),
                    (normalizedProcessor, 2, 1)
                };

            int score = 0;

            foreach (var queryToken in queryTokens)
            {
                foreach (var field in fields)
                {
                    score += GetScore(queryToken, field.value, field.exact, field.partial);
                }
            }

            if (normalizedName.Contains(normalizedQuery))
            {
                score += 20;
            }

            if (combined.Contains(normalizedQuery))
            {
                score += 10;
            }

            return score;
        }

        private static int GetScore(string queryToken, string field, int exact, int partial)
        {
            if (string.IsNullOrWhiteSpace(queryToken) || string.IsNullOrWhiteSpace(field))
            {
                return 0;
            }

            var fieldWords = field.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (fieldWords.Contains(queryToken))
            {
                return exact;
            }

            if (queryToken.Length >= 3 && field.Contains(queryToken))
            {
                return partial;
            }

            return 0;
        }

        public static string NormalizeRam(decimal? ramAmount)
        {
            if (ramAmount == null)
            {
                return string.Empty;
            }

            var ramText = ramAmount % 1 == 0
                ? ((int)ramAmount.Value).ToString()
                : ramAmount.Value.ToString(System.Globalization.CultureInfo.InvariantCulture);

            return NormalizeInput($"{ramText} gb");
        }

        public static string NormalizeInput(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return string.Empty;
            }

            input = input.ToLowerInvariant();

            var sb = new StringBuilder();

            for (int i = 0; i < input.Length; i++)
            {
                var ch = input[i];

                if (char.IsLetterOrDigit(ch) || char.IsWhiteSpace(ch))
                {
                    if (i > 0)
                    {
                        var prev = input[i - 1];

                        if ((char.IsLetter(prev) && char.IsDigit(ch)) || (char.IsDigit(prev) && char.IsLetter(ch)))
                        {
                            sb.Append(' ');
                        }
                    }

                    sb.Append(ch);
                }
                else
                {
                    sb.Append(' ');
                }
            }

            return Regex.Replace(sb.ToString(), @"\s+", " ").Trim();
        }

        public static List<string> CreateSearchTokens(string normalizedInput)
        {
            if (string.IsNullOrWhiteSpace(normalizedInput))
            {
                return new List<string>();
            }

            return normalizedInput
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Distinct()
                .ToList();
        }
    }
}
