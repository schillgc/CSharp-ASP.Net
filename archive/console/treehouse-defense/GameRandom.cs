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
        if (!double.IsFinite(probability) || probability is < 0 or > 1)
        {
            throw new ArgumentOutOfRangeException(nameof(probability), "Probability must be between 0 and 1.");
        }

        return NextDouble() < probability;
    }
}
