namespace TreehouseDefense;

public sealed class Path
{
    private readonly MapLocation[] _locations;

    public int Length => _locations.Length;
    public Map Map => _locations[0].Map;

    public Path(IEnumerable<MapLocation> locations)
    {
        ArgumentNullException.ThrowIfNull(locations);
        _locations = locations.ToArray();

        if (_locations.Length == 0)
        {
            throw new ArgumentException("A path must contain at least one location.", nameof(locations));
        }

        if (_locations.Any(location => location is null))
        {
            throw new ArgumentException("A path cannot contain null locations.", nameof(locations));
        }

        var map = _locations[0].Map;
        if (_locations.Any(location => !ReferenceEquals(location.Map, map)))
        {
            throw new ArgumentException("Every path location must belong to the same map.", nameof(locations));
        }
    }

    public MapLocation? GetLocationAt(int pathStep) =>
        pathStep >= 0 && pathStep < _locations.Length ? _locations[pathStep] : null;

    public bool IsOnPath(MapLocation location)
    {
        ArgumentNullException.ThrowIfNull(location);
        return _locations.Contains(location);
    }
}
