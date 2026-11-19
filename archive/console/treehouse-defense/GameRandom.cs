namespace TreehouseDefense;

/// <summary>A small, seedable random source so simulations can be reproduced.</summary>
public sealed class GameRandom
{
    private readonly Random _random;

    public GameRandom(int seed)
    {
        _random = new Random(seed);
    }

    public double NextDouble() => _random.NextDouble();

    public bool Chance(double probability)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(probability);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(probability, 1);
        return NextDouble() < probability;
    }
}
