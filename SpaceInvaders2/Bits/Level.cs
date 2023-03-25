using System.Drawing;

namespace SpaceInvaders2.Bits;

public class Level
{
    public List<Enemy> Enemies { get; set; }
    public List<Obstacle> Obstacles { get; set; }
    public int Speed { get; set; }
    public List<Projectile> Projectiles { get; set; }

    public Level(List<Enemy> enemies, List<Obstacle> obstacles, int speed, List<Projectile> projectiles)
    {
        Enemies = enemies;
        Obstacles = obstacles;
        Speed = speed;
        Projectiles = projectiles;
    }
}

public class Obstacle
{
    public Rectangle Rectangle { get; set; }
    public Color Color { get; set; }

    public Obstacle(Rectangle rectangle, Color color)
    {
        Rectangle = rectangle;
        Color = color;
    }
}