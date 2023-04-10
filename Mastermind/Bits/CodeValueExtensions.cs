using System.Drawing;

namespace MastermindGame.Bits;

public static class CodeValueExtensions
{
    public static Color ToColor(this CodeValue? codeValue)
    {
        return codeValue == null ? Color.Black : Colors[(int)codeValue];
    }

    private static readonly Color[] Colors = new[]
    {
        Color.White,
        Color.Yellow,
        Color.Orange,
        Color.Red,
        Color.Green,
        Color.Purple
    };
}