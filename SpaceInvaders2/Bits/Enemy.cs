using System.Drawing;
using Core.Display.Sprites;

namespace SpaceInvaders2.Bits;

public class Enemy
{
    public Rectangle Rectangle { get; set; }
    public SpriteAnimation Sprite { get; set; }
    public int Health { get; set; }
    public bool IsStealth { get; set; }
    public int ProjectileType { get; set; }

    public Enemy(Rectangle rectangle, SpriteAnimation sprite, int health, bool isStealth, int projectileType)
    {
        Rectangle = rectangle;
        Sprite = sprite;
        Health = health;
        IsStealth = isStealth;
        ProjectileType = projectileType;
    }
}