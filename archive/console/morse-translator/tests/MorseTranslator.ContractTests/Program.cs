using SchillingEnterprises.MorseTranslator;

var tests = new (string Name, Action Run)[]
{
    ("encodes and decodes SOS", () =>
    {
        Equal("... --- ...", MorseCodec.Encode("SOS"));
        Equal("SOS", MorseCodec.Decode("... --- ..."));
    }),
    ("round-trips multiple words and punctuation", () =>
    {
        const string message = "Hello, world! 123";
        Equal(message.ToUpperInvariant(), MorseCodec.Decode(MorseCodec.Encode(message)));
    }),
    ("round-trips the recovered extended Latin alphabet", () =>
    {
        const string message = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789ÁÉÖÄÑÜ";
        Equal(message, MorseCodec.Decode(MorseCodec.Encode(message)));
    }),
    ("collapses repeated whitespace into a word separator", () =>
    {
        Equal(".... . .-.. .-.. --- / .-- --- .-. .-.. -..", MorseCodec.Encode("HELLO\t  WORLD"));
        Equal("HELLO WORLD", MorseCodec.Decode(".... . .-.. .-.. --- / .-- --- .-. .-.. -.."));
    }),
    ("returns empty output for empty input", () =>
    {
        Equal(string.Empty, MorseCodec.Encode(" \t "));
        Equal(string.Empty, MorseCodec.Decode("  "));
    }),
    ("rejects unsupported text characters", () =>
    {
        Throws<FormatException>(() => MorseCodec.Encode("HELLO 🌍"));
    }),
    ("rejects unknown Morse symbols", () =>
    {
        Throws<FormatException>(() => MorseCodec.Decode("........"));
    }),
    ("rejects empty Morse word segments", () =>
    {
        Throws<FormatException>(() => MorseCodec.Decode("... --- ... // .... . .-.. .--."));
    }),
    ("rejects null inputs", () =>
    {
        Throws<ArgumentNullException>(() => MorseCodec.Encode(null!));
        Throws<ArgumentNullException>(() => MorseCodec.Decode(null!));
    })
};

var failures = 0;
foreach (var test in tests)
{
    try
    {
        test.Run();
        Console.WriteLine($"PASS {test.Name}");
    }
    catch (Exception exception)
    {
        failures++;
        Console.Error.WriteLine($"FAIL {test.Name}: {exception.Message}");
    }
}

Console.WriteLine($"{tests.Length - failures}/{tests.Length} contract tests passed.");
return failures == 0 ? 0 : 1;

static void Equal<T>(T expected, T actual)
{
    if (!EqualityComparer<T>.Default.Equals(expected, actual))
    {
        throw new InvalidOperationException($"Expected '{expected}', received '{actual}'.");
    }
}

static void Throws<TException>(Action action) where TException : Exception
{
    try
    {
        action();
    }
    catch (TException)
    {
        return;
    }

    throw new InvalidOperationException($"Expected {typeof(TException).Name}.");
}
