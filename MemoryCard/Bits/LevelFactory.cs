using System.Drawing;

namespace MemoryCard.Bits;

public class LevelFactory
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
    
    public static Level CreateLevel(int level)
    {
        return level switch
        {
            1 => new Level(4, 4, AllShapes, new[] { Color.Blue }),
            _ => throw new ArgumentOutOfRangeException(nameof(level), level, null)
        };
    }
}