using TreehouseDefense;
using Path = TreehouseDefense.Path;

var tests = new (string Name, Action Run)[]
{
    ("Map rejects non-positive dimensions", MapRejectsInvalidDimensions),
    ("Map locations reject out-of-bounds coordinates", MapLocationRejectsOutOfBounds),
    ("Path rejects empty and mixed-map locations", PathRejectsInvalidLocations),
    ("Path rejects null locations", PathRejectsNullLocations),
    ("Map distance remains safe for large coordinates", LargeMapDistanceDoesNotOverflow),
    ("Fast invader advances two path steps", FastInvaderMovesTwoSteps),
    ("Sniper tower deals guaranteed damage in range", SniperTowerHitsInRange),
    ("Level loses when an invader reaches the path end", LevelDetectsScoringInvader),
    ("Level loses when an invader has already scored", LevelDetectsPreviouslyScoredInvader),
    ("Level enforces its maximum turn count", LevelEnforcesMaximumTurns),
    ("Level wins when all invaders are neutralized", LevelDetectsNeutralizedInvaders),
    ("Resurrecting invaders reject invalid damage", ResurrectingInvaderRejectsInvalidDamage),
    ("Seeded simulations are reproducible", SeededSimulationIsReproducible),
    ("Random probabilities and tower accuracy reject non-finite values", RejectsNonFiniteProbabilities)
};

var failures = new List<string>();
foreach (var (name, run) in tests)
{
    try
    {
        run();
        Console.WriteLine($"PASS {name}");
    }
    catch (Exception exception)
    {
        failures.Add($"{name}: {exception.Message}");
        Console.WriteLine($"FAIL {name}: {exception.Message}");
    }
}

Console.WriteLine($"{tests.Length - failures.Count}/{tests.Length} contract tests passed.");
return failures.Count == 0 ? 0 : 1;

static void MapRejectsInvalidDimensions()
{
    AssertThrows<ArgumentOutOfRangeException>(() => new Map(0, 4));
    AssertThrows<ArgumentOutOfRangeException>(() => new Map(4, -1));
}

static void MapLocationRejectsOutOfBounds()
{
    var map = new Map(3, 3);
    AssertThrows<OutOfBoundsException>(() => new MapLocation(3, 1, map));
    AssertThrows<OutOfBoundsException>(() => new MapLocation(-1, 0, map));
}

static void PathRejectsInvalidLocations()
{
    AssertThrows<ArgumentException>(() => new Path(Array.Empty<MapLocation>()));

    var firstMap = new Map(3, 3);
    var secondMap = new Map(3, 3);
    AssertThrows<ArgumentException>(() => new Path(
    [
        new MapLocation(0, 0, firstMap),
        new MapLocation(1, 0, secondMap)
    ]));
}

static void PathRejectsNullLocations()
{
    MapLocation[] invalidLocations = [null!];
    AssertThrows<ArgumentException>(() => new Path(invalidLocations));
}

static void LargeMapDistanceDoesNotOverflow()
{
    var map = new Map(int.MaxValue, 1);
    var left = new MapLocation(0, 0, map);
    var right = new MapLocation(1_500_000_000, 0, map);
    AssertEqual(1_500_000_000d, left.DistanceTo(right));
}

static void FastInvaderMovesTwoSteps()
{
    var path = BuildPath(5);
    var invader = new FastInvader(path);
    invader.Move();
    AssertEqual(new MapLocation(2, 0, path.Map), invader.Location);
}

static void SniperTowerHitsInRange()
{
    var map = new Map(4, 2);
    var location = new MapLocation(0, 0, map);
    var path = new Path([new MapLocation(1, 0, map), new MapLocation(2, 0, map)]);
    var invader = new BasicInvader(path);
    var tower = StandardTowers.Sniper(location, new GameRandom(7));

    AssertTrue(tower.FireOnInvaders([invader]));
    AssertEqual(1, invader.Health);
    AssertTrue(tower.FireOnInvaders([invader]));
    AssertTrue(invader.IsNeutralized);
}

static void LevelDetectsScoringInvader()
{
    var path = BuildPath(2);
    var level = new Level([new BasicInvader(path)], Array.Empty<Tower>());
    AssertFalse(level.Play());
    AssertEqual(2, level.TurnsPlayed);
}

static void LevelDetectsPreviouslyScoredInvader()
{
    var path = BuildPath(2);
    var invader = new FastInvader(path);
    invader.Move();
    AssertTrue(invader.HasScored);

    var level = new Level([invader], Array.Empty<Tower>());
    AssertFalse(level.Play());
    AssertEqual(0, level.TurnsPlayed);
}

static void LevelEnforcesMaximumTurns()
{
    var path = BuildPath(4);
    var level = new Level([new BasicInvader(path)], Array.Empty<Tower>());
    AssertThrows<TimeoutException>(() => level.Play(maximumTurns: 1));
    AssertEqual(1, level.TurnsPlayed);
}

static void LevelDetectsNeutralizedInvaders()
{
    var map = new Map(3, 1);
    var path = new Path([new MapLocation(0, 0, map), new MapLocation(1, 0, map)]);
    var invader = new BasicInvader(path);
    var tower = StandardTowers.Sniper(new MapLocation(0, 0, map), new GameRandom(9));
    var level = new Level([invader], [tower]);

    AssertTrue(level.Play());
    AssertTrue(invader.IsNeutralized);
}

static void ResurrectingInvaderRejectsInvalidDamage()
{
    var invader = new ResurrectingInvader(BuildPath(3));

    // The first form has 2 health; after it is neutralized, the stronger
    // second form begins with 4 health and must be defeated separately.
    invader.DecreaseHealth(2);
    AssertFalse(invader.IsNeutralized);
    AssertEqual(4, invader.Health);

    invader.DecreaseHealth(4);
    AssertTrue(invader.IsNeutralized);
    AssertThrows<ArgumentOutOfRangeException>(() => invader.DecreaseHealth(0));
}

static void SeededSimulationIsReproducible()
{
    static (bool Won, int Turns) Run(int seed)
    {
        var random = new GameRandom(seed);
        var map = new Map(5, 2);
        var path = new Path(Enumerable.Range(0, 5).Select(x => new MapLocation(x, 0, map)));
        var invaders = new IInvader[]
        {
            new ShieldedInvader(path, random),
            new BasicInvader(path),
            new FastInvader(path)
        };
        var towers = new[]
        {
            StandardTowers.Sniper(new MapLocation(1, 1, map), random),
            StandardTowers.Powerful(new MapLocation(3, 1, map), random)
        };
        var level = new Level(invaders, towers);
        return (level.Play(), level.TurnsPlayed);
    }

    AssertEqual(Run(1234), Run(1234));
}

static void RejectsNonFiniteProbabilities()
{
    var random = new GameRandom(42);
    foreach (var probability in new[] { double.NaN, double.PositiveInfinity, double.NegativeInfinity })
    {
        AssertThrows<ArgumentOutOfRangeException>(() => random.Chance(probability));
        AssertThrows<ArgumentOutOfRangeException>(() => new Tower(
            new MapLocation(0, 0, new Map(1, 1)), random, accuracy: probability));
    }

    AssertTrue(random.Chance(0));
    AssertFalse(random.Chance(1));
}

static Path BuildPath(int length)
{
    var map = new Map(length, 1);
    return new Path(Enumerable.Range(0, length).Select(x => new MapLocation(x, 0, map)));
}

static void AssertTrue(bool value)
{
    if (!value) throw new InvalidOperationException("Expected true.");
}

static void AssertFalse(bool value)
{
    if (value) throw new InvalidOperationException("Expected false.");
}

static void AssertEqual<T>(T expected, T actual)
{
    if (!EqualityComparer<T>.Default.Equals(expected, actual))
    {
        throw new InvalidOperationException($"Expected '{expected}', received '{actual}'.");
    }
}

static void AssertThrows<TException>(Action action) where TException : Exception
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
