using System.Drawing;

namespace MemoryCard.Bits;

public static class LevelFactory
{
    private static readonly CardShape[] AllShapes = new[]
    {
        CardShape.Circle,
        CardShape.Triangle,
        CardShape.Square,
        CardShape.Diamond,
        CardShape.Star,
        CardShape.Plus,
        CardShape.Cross,
        CardShape.Hexagon
    };
    
    public static int MaxLevel => 4;
    
    public static Level GetLevel(int level)
    {
        return level switch
        {
            1 => new Level(1, 2, 4, AllShapes, new[] { Color.Blue }),
            2 => new Level(2, 3, 4, AllShapes, new[] { Color.Blue }),
            3 => new Level(3, 4, 4, AllShapes, new[] { Color.Blue }),
            4 => new Level(4, 5, 5, AllShapes, new[] { Color.Blue }),
            _ => throw new ArgumentOutOfRangeException(nameof(level), level, null)
        };
    }
}