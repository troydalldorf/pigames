using System.Drawing;
using Core;
using Core.Display.Sprites;
using Core.Sprites;

namespace Frogger.Bits;

public class Vehicle
{
    private const int VehicleHeight = 4;

    private Rectangle rectangle;
    private readonly ISprite vehicleSprite;

    public Vehicle(int x, int y, int width, ISprite sprite)
    {
        rectangle = new Rectangle(x, y, width, VehicleHeight);
        vehicleSprite = sprite;
    }

    public void Update(int speed)
    {
        rectangle.X += speed;
    }

    public void Draw(IDisplay display)
    {
        var srcRectangle = new Rectangle(0, 0, rectangle.Width, rectangle.Height);
        vehicleSprite.DrawTiled(display, srcRectangle);
    }

    public Rectangle Rectangle => rectangle;
}