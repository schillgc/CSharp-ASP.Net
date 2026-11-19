namespace TreehouseDefense;

public sealed class Tower
{
    private readonly GameRandom _random;

    public MapLocation Location { get; }
    public int Range { get; }
    public int Power { get; }
    public double Accuracy { get; }

    public Tower(MapLocation location, GameRandom random, int range = 1, int power = 1, double accuracy = 0.75)
    {
        Location = location ?? throw new ArgumentNullException(nameof(location));
        _random = random ?? throw new ArgumentNullException(nameof(random));
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(range);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(power);
        if (accuracy is < 0 or > 1)
        {
            throw new ArgumentOutOfRangeException(nameof(accuracy), "Accuracy must be between 0 and 1.");
        }

        Range = range;
        Power = power;
        Accuracy = accuracy;
    }

    /// <summary>Fires at the first active invader in range; returns false if none is in range.</summary>
    public bool FireOnInvaders(IEnumerable<IInvader> invaders)
    {
        ArgumentNullException.ThrowIfNull(invaders);

        foreach (var invader in invaders)
        {
            var target = invader.Location;
            if (!invader.IsActive || target is null || !ReferenceEquals(target.Map, Location.Map))
            {
                continue;
            }

            if (!Location.InRangeOf(target, Range))
            {
                continue;
            }

            if (_random.Chance(Accuracy))
            {
                invader.DecreaseHealth(Power);
            }

            return true;
        }

        return false;
    }
}

public static class StandardTowers
{
    public static Tower Sniper(MapLocation location, GameRandom random) =>
        new(location, random, range: 2, power: 1, accuracy: 1);

    public static Tower Powerful(MapLocation location, GameRandom random) =>
        new(location, random, range: 1, power: 2, accuracy: 0.75);

    public static Tower LongRange(MapLocation location, GameRandom random) =>
        new(location, random, range: 2, power: 1, accuracy: 0.5);
}
