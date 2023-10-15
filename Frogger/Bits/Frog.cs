using System.Drawing;
using Core;
using Core.Display;
using Core.Display.Sprites;

namespace Frogger.Bits;

public class Frog
{
    private const int FrogSize = 4;
    private const int LaneHeight = 8;
    private const int Width = 64;
    private const int Height = 64;

    private Rectangle rectangle;
    private ISprite frogSprite;
    public bool IsAtTop => rectangle.Y == 0;

    public Frog(int screenWidth, int screenHeight)
    {
        var image = SpriteImage.FromResource("frogger.png", new Point(1, 1));
        frogSprite = image.GetSprite(1, 1, 4, 4);
        ResetPosition();
    }

    public void HandleInput(JoystickDirection stick)
    {
        if (stick.IsUp()) rectangle.Y--;
        if (stick.IsDown()) rectangle.Y++;
        if (stick.IsLeft()) rectangle.X--;
        if (stick.IsRight()) rectangle.X++;

        rectangle.X = Math.Clamp(rectangle.X, 0, Width - FrogSize);
        rectangle.Y = Math.Clamp(rectangle.Y, 0, Height - LaneHeight);
    }

    public void ResetPosition()
    {
        rectangle = new Rectangle(Width / 2 - FrogSize / 2, Height - LaneHeight, FrogSize, FrogSize);
    }

    public void Draw(IDisplay display)
    {
        frogSprite.Draw(display, rectangle.X, rectangle.Y);
    }

    public Rectangle Rectangle => rectangle;
}