namespace TreehouseDefense;

public sealed class MapLocation : IEquatable<MapLocation>
{
    public int X { get; }
    public int Y { get; }
    public Map Map { get; }

    public MapLocation(int x, int y, Map map)
    {
        ArgumentNullException.ThrowIfNull(map);
        if (!map.Contains(x, y))
        {
            throw new OutOfBoundsException($"Location ({x}, {y}) is outside the {map.Width}x{map.Height} map.");
        }

        X = x;
        Y = y;
        Map = map;
    }

    public double DistanceTo(MapLocation other)
    {
        ArgumentNullException.ThrowIfNull(other);
        var deltaX = (double)X - other.X;
        var deltaY = (double)Y - other.Y;
        return Math.Sqrt((deltaX * deltaX) + (deltaY * deltaY));
    }

    public bool InRangeOf(MapLocation other, int range) =>
        range >= 0 && ReferenceEquals(Map, other.Map) && DistanceTo(other) <= range;

    public bool Equals(MapLocation? other) =>
        other is not null && X == other.X && Y == other.Y && ReferenceEquals(Map, other.Map);

    public override bool Equals(object? obj) => Equals(obj as MapLocation);

    public override int GetHashCode() => HashCode.Combine(X, Y, Map);

    public override string ToString() => $"({X}, {Y})";
}

public class TreehouseDefenseException : Exception
{
    public TreehouseDefenseException(string message) : base(message) { }
}

public sealed class OutOfBoundsException : TreehouseDefenseException
{
    public OutOfBoundsException(string message) : base(message) { }
}
