using System.Globalization;

namespace FitnessFrog;

public enum EntryKind
{
    Valid,
    Quit,
    Invalid
}

public readonly record struct ExerciseEntryResult(EntryKind Kind, double Minutes, string? Error)
{
    public static ExerciseEntryResult Valid(double minutes) => new(EntryKind.Valid, minutes, null);
    public static ExerciseEntryResult Quit() => new(EntryKind.Quit, 0, null);
    public static ExerciseEntryResult Invalid(string error) => new(EntryKind.Invalid, 0, error);
}

public static class ExerciseEntryParser
{
    public static ExerciseEntryResult Parse(string? input)
    {
        if (input is null)
        {
            return ExerciseEntryResult.Quit();
        }

        var value = input.Trim();
        if (value.Equals("quit", StringComparison.OrdinalIgnoreCase))
        {
            return ExerciseEntryResult.Quit();
        }

        if (!double.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out var minutes)
            || !double.IsFinite(minutes))
        {
            return ExerciseEntryResult.Invalid("Enter a finite number of minutes or type 'quit'.");
        }

        if (minutes <= 0)
        {
            return ExerciseEntryResult.Invalid("Exercise time must be greater than zero.");
        }

        return ExerciseEntryResult.Valid(minutes);
    }
}
