using System.Drawing;
using Core;

namespace BomberMan.Bits;

public class Player
{
    public int X { get; private set; }
    public int Y { get; private set; }
    private readonly Grid grid;

    public Player(int x, int y, Grid grid)
    {
        X = x;
        Y = y;
        this.grid = grid;
    }

    public void Move(int dx, int dy)
    {
        int newX = X + dx;
        int newY = Y + dy;
        if (newX >= 0 && newX < grid.Width && newY >= 0 && newY < grid.Height && !grid.IsWall(newX, newY))
        {
            X = newX;
            Y = newY;
        }
    }

    public void Draw(IDisplay display, Color color)
    {
        display.DrawRectangle(X * 8, Y * 8, 8, 8, color, color);
    }
}