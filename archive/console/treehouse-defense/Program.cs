namespace TreehouseDefense;

internal static class Program
{
    private static int Main(string[] args)
    {
        var seed = ReadSeed(args);
        var random = new GameRandom(seed);
        var map = new Map(8, 5);

        try
        {
            var path = new Path(
            [
                new MapLocation(0, 2, map),
                new MapLocation(1, 2, map),
                new MapLocation(2, 2, map),
                new MapLocation(3, 2, map),
                new MapLocation(4, 2, map),
                new MapLocation(5, 2, map),
                new MapLocation(6, 2, map),
                new MapLocation(7, 2, map)
            ]);

            IInvader[] invaders =
            [
                new ShieldedInvader(path, random),
                new FastInvader(path),
                new StrongInvader(path),
                new BasicInvader(path),
                new ResurrectingInvader(path)
            ];

            Tower[] towers =
            [
                StandardTowers.Sniper(new MapLocation(1, 3, map), random),
                StandardTowers.Powerful(new MapLocation(3, 3, map), random),
                StandardTowers.LongRange(new MapLocation(5, 3, map), random)
            ];

            var level = new Level(invaders, towers);
            var playerWon = level.Play();

            Console.WriteLine($"Seed: {seed}");
            Console.WriteLine($"Turns played: {level.TurnsPlayed}");
            Console.WriteLine(playerWon ? "Player won!" : "Player lost.");
            return playerWon ? 0 : 1;
        }
        catch (TreehouseDefenseException exception)
        {
            Console.Error.WriteLine(exception.Message);
            return 2;
        }
    }

    private static int ReadSeed(string[] args)
    {
        if (args.Length == 0)
        {
            return 42;
        }

        if (args.Length == 2 && args[0] == "--seed" && int.TryParse(args[1], out var seed))
        {
            return seed;
        }

        Console.Error.WriteLine("Usage: TreehouseDefense [--seed INTEGER]");
        Environment.ExitCode = 2;
        return 42;
    }
}
