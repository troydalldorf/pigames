using System.Drawing;
using Core;
using Core.Display;
using Core.Display.Sprites;

namespace BomberMan.Bits;

public class Player : IGameElement
{
    public int X { get; private set; }
    public int Y { get; private set; }
    private readonly Grid grid;
    private readonly Sprite sprite;

    public Player(int x, int y, Grid grid, Sprite sprite)
    {
        X = x;
        Y = y;
        this.grid = grid;
        this.sprite = sprite;
    }

    public void Move(int dx, int dy)
    {
        var newX = X + dx;
        var newY = Y + dy;
        if (newX >= 0 && newX < grid.Width && newY >= 0 && newY < grid.Height && !grid.IsWall(newX, newY))
        {
            X = newX;
            Y = newY;
        }
    }

    public void Update()
    {
    }

    public void Draw(IDisplay display)
    {
        sprite.Draw(display, X*8, Y*8);
    }
}