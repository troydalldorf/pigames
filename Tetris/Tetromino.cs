namespace Tetris;

using System;
using System.Drawing;

class Tetromino
{
    private static readonly int[][] TetrominoShapes = new int[][]
    {
        new int[] { 0x0F00, 0x2222, 0x00F0, 0x4444 }, // I
        new int[] { 0x8E00, 0x6440, 0x0E20, 0x44C0 }, // L
        new int[] { 0x2E00, 0x4460, 0x0E80, 0xC440 }, // J
        new int[] { 0x6600, 0x6600, 0x6600, 0x6600 }, // O
        new int[] { 0x6C00, 0x4620, 0x06C0, 0x8C40 }, // S
        new int[] { 0xC600, 0x2640, 0x0C60, 0x4C80 }, // Z
        new int[] { 0x4E00, 0x4640, 0x0E40, 0x4C40 }, // T
    };

    private static readonly Color[] TetrominoColors = new Color[]
    {
        Color.Cyan,    // I
        Color.Orange,  // L
        Color.Blue,    // J
        Color.Yellow,  // O
        Color.Green,   // S
        Color.Red,     // Z
        Color.Purple,  // T
    };

    private int[] shape;
    private int rotation;
    public int Type { get; }

    public Tetromino(int type)
    {
        Type = type;
        shape = TetrominoShapes[type];
        rotation = 0;
    }

    public int Width => 4;
    public int Height => 4;

    public bool this[int x, int y] => (shape[rotation] & (1 << (15 - (y * 4 + x)))) != 0;

    public static Tetromino RandomTetromino(Random random)
    {
        return new Tetromino(random.Next(TetrominoShapes.Length));
    }

    public static Color GetColor(int type)
    {
        return TetrominoColors[type];
    }

    public Tetromino RotateRight()
    {
        Tetromino rotated = new Tetromino(Type)
        {
            shape = shape,
            rotation = (rotation + 1) % 4
        };
        return rotated;
    }
}
