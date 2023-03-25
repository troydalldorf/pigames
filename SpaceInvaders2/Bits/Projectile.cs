using System.Drawing;

namespace SpaceInvaders2.Bits;

public class Projectile
{
    public Rectangle Rectangle { get; set; }
    public Color Color { get; set; }
    public int Type { get; set; }

    public Projectile(Rectangle rectangle, Color color, int type)
    {
        Rectangle = rectangle;
        Color = color;
        Type = type;
    }
}