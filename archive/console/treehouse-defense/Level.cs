namespace TreehouseDefense;

public sealed class Level
{
    private readonly IInvader[] _invaders;
    private readonly Tower[] _towers;

    public int TurnsPlayed { get; private set; }

    public Level(IEnumerable<IInvader> invaders, IEnumerable<Tower> towers)
    {
        ArgumentNullException.ThrowIfNull(invaders);
        ArgumentNullException.ThrowIfNull(towers);
        _invaders = invaders.ToArray();
        _towers = towers.ToArray();

        if (_invaders.Any(invader => invader is null))
        {
            throw new ArgumentException("Invader collections cannot contain null values.", nameof(invaders));
        }

        if (_towers.Any(tower => tower is null))
        {
            throw new ArgumentException("Tower collections cannot contain null values.", nameof(towers));
        }
    }

    /// <summary>Returns true when every invader is neutralized, false when one reaches the end.</summary>
    public bool Play(int maximumTurns = 1_000)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(maximumTurns);
        TurnsPlayed = 0;

        // A previously scored invader is already a loss; do not mistake an
        // inactive, scored collection for a completed/won level.
        if (_invaders.Any(invader => invader.HasScored))
        {
            return false;
        }

        while (_invaders.Any(invader => invader.IsActive))
        {
            if (TurnsPlayed >= maximumTurns)
            {
                throw new TimeoutException($"Level did not finish within {maximumTurns} turns.");
            }

            foreach (var tower in _towers)
            {
                tower.FireOnInvaders(_invaders);
            }

            foreach (var invader in _invaders.Where(invader => invader.IsActive))
            {
                invader.Move();
                if (invader.HasScored)
                {
                    TurnsPlayed++;
                    return false;
                }
            }

            TurnsPlayed++;
        }

        return true;
    }
}
