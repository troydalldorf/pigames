using System.Drawing;
using Core;
using Core.Display;

namespace BomberMan.Bits;

public class Grid
{
    private readonly bool[,] maze;

    public Grid(int width, int height)
    {
        Width = width;
        Height = height;
        maze = new bool[width, height];
        GenerateMaze();
    }
    
    public int Width { get; private set; }
    public int Height { get; private set; }

    private void GenerateMaze()
    {
        // Hard-coded maze layout (1 for walls, 0 for paths)
        int[,] mazeLayout =
        {
            { 1, 0, 1, 1, 1, 1, 0, 1 },
            { 1, 0, 1, 0, 0, 0, 0, 1 },
            { 1, 0, 0, 0, 1, 1, 0, 1 },
            { 1, 1, 1, 0, 1, 1, 0, 1 },
            { 1, 1, 1, 0, 1, 1, 0, 1 },
            { 1, 0, 0, 0, 0, 0, 0, 1 },
            { 1, 0, 1, 1, 1, 1, 0, 1 },
            { 1, 0, 1, 1, 1, 1, 0, 1 }
        };
        for (var x = 0; x < Width; x++)
        {
            for (var y = 0; y < Height; y++)
            {
                maze[x, y] = mazeLayout[y, x] == 1;
            }
        }
    }

    public bool IsWall(int x, int y)
    {
        return maze[x, y];
    }

    public void Draw(IDisplay display)
    {
        for (var x = 0; x < Width; x++)
        {
            for (var y = 0; y < Height; y++)
            {
                if (maze[x, y])
                {
                    display.DrawRectangle(x * 8, y * 8, 8, 8, Color.Gray, Color.Gray);
                }
            }
        }
    }
}