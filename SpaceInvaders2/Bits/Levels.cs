using System.Drawing;
using Core.Display.Sprites;

namespace SpaceInvaders2.Bits;

public class Levels
{
    private const int Width = 64;
    private const int Height = 64;
    private const int InvaderWidth = 4;
    public const int InvaderHeight = 3;
    public const int BulletWidth = 1;
    public const int BulletHeight = 3;
    
    private readonly SpriteAnimation alienSprite;

    public Levels(SpriteImage image)
    {
        alienSprite = image.GetSpriteAnimation(0, 0, 4, 3, 2, 1);
    }

    public Level CreateLevel1()
    {
        var enemies = new List<Enemy>();
        for (var y = 0; y < 5; y++)
        {
            for (var x = 0; x < 10; x++)
            {
                var rectangle = new Rectangle(x * 6, y * 4, InvaderWidth, InvaderHeight);
                var enemy = new Enemy(rectangle, alienSprite, 1, false, 0);
                enemies.Add(enemy);
            }
        }

        var obstacles = new List<Obstacle>();
        obstacles.Add(new Obstacle(new Rectangle(Width / 2, Height / 2, 10, 10), Color.Gray));

        var projectiles = new List<Projectile>();
        projectiles.Add(new Projectile(new Rectangle(0, 0, BulletWidth, BulletHeight), Color.Red, 0));

        return new Level(enemies, obstacles, 1, projectiles);
    }
}