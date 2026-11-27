namespace FitnessFrog;

public sealed class FitnessSession
{
    public double TotalMinutes { get; private set; }
    public int EntryCount { get; private set; }

    public string Add(double minutes)
    {
        if (!double.IsFinite(minutes) || minutes <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(minutes), "Exercise minutes must be finite and greater than zero.");
        }

        var updatedEntryCount = checked(EntryCount + 1);
        var updatedTotal = TotalMinutes + minutes;
        if (!double.IsFinite(updatedTotal))
        {
            throw new OverflowException("The cumulative exercise total exceeds the supported numeric range.");
        }

        TotalMinutes = updatedTotal;
        EntryCount = updatedEntryCount;
        return ActivityFeedback.For(minutes);
    }
}

public static class ActivityFeedback
{
    public static string For(double minutes)
    {
        if (!double.IsFinite(minutes) || minutes <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(minutes), "Exercise minutes must be finite and greater than zero.");
        }

        return minutes switch
        {
            <= 10 => "Better than nothing, am I right?",
            <= 30 => "Way to go!",
            <= 60 => "You must be a ninja warrior in training!",
            _ => "Okay, now you're just showing off!"
        };
    }
}
