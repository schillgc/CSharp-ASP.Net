using System.Text;

namespace SchillingEnterprises.MorseTranslator;

/// <summary>
/// Converts supported Latin letters, digits, and punctuation to and from International Morse code.
/// Words are separated by a slash in encoded text; symbols within a word are separated by spaces.
/// </summary>
public static class MorseCodec
{
    private static readonly IReadOnlyDictionary<char, string> TextToMorse =
        new Dictionary<char, string>
        {
            [' '] = "/",
            ['A'] = ".-", ['B'] = "-...", ['C'] = "-.-.", ['D'] = "-..",
            ['E'] = ".", ['F'] = "..-.", ['G'] = "--.", ['H'] = "....",
            ['I'] = "..", ['J'] = ".---", ['K'] = "-.-", ['L'] = ".-..",
            ['M'] = "--", ['N'] = "-.", ['O'] = "---", ['P'] = ".--.",
            ['Q'] = "--.-", ['R'] = ".-.", ['S'] = "...", ['T'] = "-",
            ['U'] = "..-", ['V'] = "...-", ['W'] = ".--", ['X'] = "-..-",
            ['Y'] = "-.--", ['Z'] = "--..",
            ['0'] = "-----", ['1'] = ".----", ['2'] = "..---", ['3'] = "...--",
            ['4'] = "....-", ['5'] = ".....", ['6'] = "-....", ['7'] = "--...",
            ['8'] = "---..", ['9'] = "----.",
            [','] = "--..--", ['.'] = ".-.-.-", ['?'] = "..--..", [';'] = "-.-.-.",
            [':'] = "---...", ['/'] = "-..-.", ['-'] = "-....-", ['\''] = ".----.",
            ['"'] = ".-..-.", ['('] = "-.--.", [')'] = "-.--.-", ['='] = "-...-",
            ['+'] = ".-.-.", ['@'] = ".--.-.", ['!'] = "-.-.--",
            ['Á'] = ".--.-", ['É'] = "..-..", ['Ö'] = "---.", ['Ä'] = ".-.-",
            ['Ñ'] = "--.--", ['Ü'] = "..--"
        };

    private static readonly IReadOnlyDictionary<string, char> MorseToText =
        TextToMorse
            .Where(pair => pair.Key != ' ')
            .ToDictionary(pair => pair.Value, pair => pair.Key, StringComparer.Ordinal);

    /// <summary>
    /// Encodes text. Runs of whitespace become one word separator. Unsupported characters fail
    /// explicitly instead of silently producing a misleading placeholder.
    /// </summary>
    public static string Encode(string input)
    {
        ArgumentNullException.ThrowIfNull(input);

        var words = input.Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries);
        var encodedWords = new List<string>(words.Length);

        foreach (var word in words)
        {
            var symbols = new StringBuilder(word.Length);
            foreach (var character in word)
            {
                var normalized = char.ToUpperInvariant(character);
                if (!TextToMorse.TryGetValue(normalized, out var code) || normalized == ' ')
                {
                    throw new FormatException(
                        $"The character '{character}' is not supported by this Morse alphabet.");
                }

                if (symbols.Length > 0)
                {
                    symbols.Append(' ');
                }

                symbols.Append(code);
            }

            encodedWords.Add(symbols.ToString());
        }

        return string.Join(" / ", encodedWords);
    }

    /// <summary>
    /// Decodes Morse symbols separated by whitespace and words separated by a slash.
    /// Invalid or empty word segments fail explicitly.
    /// </summary>
    public static string Decode(string input)
    {
        ArgumentNullException.ThrowIfNull(input);

        var normalized = input.Trim();
        if (normalized.Length == 0)
        {
            return string.Empty;
        }

        var encodedWords = normalized.Split('/');
        if (encodedWords.Any(word => string.IsNullOrWhiteSpace(word)))
        {
            throw new FormatException("Morse words must not contain empty slash-separated segments.");
        }

        var decodedWords = new List<string>(encodedWords.Length);
        foreach (var encodedWord in encodedWords)
        {
            var symbols = encodedWord.Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries);
            if (symbols.Length == 0)
            {
                throw new FormatException("Each Morse word must contain at least one symbol.");
            }

            var decoded = new StringBuilder(symbols.Length);
            foreach (var symbol in symbols)
            {
                if (!MorseToText.TryGetValue(symbol, out var character))
                {
                    throw new FormatException($"'{symbol}' is not a recognized Morse symbol.");
                }

                decoded.Append(character);
            }

            decodedWords.Add(decoded.ToString());
        }

        return string.Join(" ", decodedWords);
    }
}
