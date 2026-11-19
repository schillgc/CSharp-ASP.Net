namespace TreehouseDefense;

public sealed class Map
{
    public int Width { get; }
    public int Height { get; }

    public Map(int width, int height)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(width);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(height);
        Width = width;
        Height = height;
    }

    public bool Contains(int x, int y) =>
        x >= 0 && x < Width && y >= 0 && y < Height;
}
