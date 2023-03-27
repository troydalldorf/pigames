using System.Drawing;

namespace MemoryCard.Bits;

public record Level(int LevelNo, int Rows, int Columns, CardShape[] Shapes, Color[] Colors);