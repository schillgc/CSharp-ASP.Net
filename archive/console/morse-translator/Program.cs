using SchillingEnterprises.MorseTranslator;

if (args.Length == 1 && (args[0] is "--help" or "-h"))
{
    Console.WriteLine("Morse Translator (.NET 10)");
    Console.WriteLine("Usage: dotnet run -- [encode|decode]");
    Console.WriteLine("Reads one line at a time from standard input; send EOF to finish.");
    return 0;
}

var decodeMode = args.Length == 1 && string.Equals(args[0], "decode", StringComparison.OrdinalIgnoreCase);
var encodeMode = args.Length == 0 || (args.Length == 1 && string.Equals(args[0], "encode", StringComparison.OrdinalIgnoreCase));

if (!decodeMode && !encodeMode)
{
    Console.Error.WriteLine("Usage: dotnet run -- [encode|decode]");
    return 2;
}

string? line;
while ((line = Console.ReadLine()) is not null)
{
    try
    {
        Console.WriteLine(decodeMode ? MorseCodec.Decode(line) : MorseCodec.Encode(line));
    }
    catch (FormatException exception)
    {
        Console.Error.WriteLine(exception.Message);
        return 2;
    }
}

return 0;
