namespace TreehouseDefense;

public interface IInvader
{
    MapLocation? Location { get; }
    bool HasScored { get; }
    int Health { get; }
    bool IsNeutralized { get; }
    bool IsActive { get; }
    void Move();
    void DecreaseHealth(int amount);
}

public abstract class Invader : IInvader
{
    private readonly Path _path;
    private int _pathStep;

    protected virtual int StepSize => 1;

    public MapLocation? Location => _path.GetLocationAt(_pathStep);
    public bool HasScored => _pathStep >= _path.Length;
    public abstract int Health { get; protected set; }
    public bool IsNeutralized => Health <= 0;
    public bool IsActive => !IsNeutralized && !HasScored;

    protected Invader(Path path)
    {
        _path = path ?? throw new ArgumentNullException(nameof(path));
    }

    public void Move()
    {
        if (IsActive)
        {
            _pathStep = Math.Min(_pathStep + StepSize, _path.Length);
        }
    }

    public virtual void DecreaseHealth(int amount)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(amount);
        if (!IsActive)
        {
            return;
        }

        Health = Math.Max(0, Health - amount);
    }
}

public sealed class BasicInvader(Path path) : Invader(path)
{
    public override int Health { get; protected set; } = 2;
}

public sealed class FastInvader(Path path) : Invader(path)
{
    protected override int StepSize => 2;
    public override int Health { get; protected set; } = 2;
}

public sealed class StrongInvader(Path path) : Invader(path)
{
    public override int Health { get; protected set; } = 4;
}

public sealed class ShieldedInvader : Invader
{
    private readonly GameRandom _random;

    public override int Health { get; protected set; } = 2;

    public ShieldedInvader(Path path, GameRandom random) : base(path)
    {
        _random = random ?? throw new ArgumentNullException(nameof(random));
    }

    public override void DecreaseHealth(int amount)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(amount);
        if (IsActive && _random.Chance(0.5))
        {
            base.DecreaseHealth(amount);
        }
    }
}

/// <summary>Two-stage invader: a basic form followed by a stronger form.</summary>
public sealed class ResurrectingInvader : IInvader
{
    private readonly BasicInvader _first;
    private readonly StrongInvader _second;

    private IInvader Current => _first.IsNeutralized ? _second : _first;

    public MapLocation? Location => Current.Location;
    public bool HasScored => Current.HasScored;
    public int Health => Current.Health;
    public bool IsNeutralized => _first.IsNeutralized && _second.IsNeutralized;
    public bool IsActive => !IsNeutralized && !HasScored;

    public ResurrectingInvader(Path path)
    {
        _first = new BasicInvader(path);
        _second = new StrongInvader(path);
    }

    public void Move()
    {
        if (IsActive)
        {
            _first.Move();
            _second.Move();
        }
    }

    public void DecreaseHealth(int amount)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(amount);
        if (IsActive)
        {
            Current.DecreaseHealth(amount);
        }
    }
}
