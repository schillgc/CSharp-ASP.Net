using System.Globalization;

namespace FitnessFrog;

internal static class Program
{
    private static int Main()
    {
        var session = new FitnessSession();

        while (true)
        {
            Console.Write("Enter minutes exercised, or type \"quit\" to finish: ");
            var input = Console.ReadLine();
            var result = ExerciseEntryParser.Parse(input);

            if (result.Kind == EntryKind.Quit)
            {
                break;
            }

            if (result.Kind == EntryKind.Invalid)
            {
                Console.WriteLine(result.Error);
                continue;
            }

            var feedback = session.Add(result.Minutes);
            Console.WriteLine(feedback);
            Console.WriteLine(
                $"Recorded {session.TotalMinutes.ToString("0.##", CultureInfo.InvariantCulture)} total minutes " +
                $"across {session.EntryCount} entries.");
        }

        Console.WriteLine($"Goodbye. Final total: {session.TotalMinutes.ToString("0.##", CultureInfo.InvariantCulture)} minutes.");
        return 0;
    }
}
