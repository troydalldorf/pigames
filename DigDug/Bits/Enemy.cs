using System.Drawing;
using Core;

namespace DigDug.Bits;

public class Enemy : IGameElement
{
    public int X { get; private set; }
    public int Y { get; private set; }

    public Enemy(int x, int y)
    {
        X = x;
        Y = y;
    }

    public void Update()
    {
        // Update enemy AI and movement
        // Implement simple enemy movement or more advanced pathfinding logic
        // Example: simple random movement
        var random = new Random();
        var direction = random.Next(0, 4);
        switch (direction)
        {
            case 0:
                X += Constants.EnemySpeed;
                break;
            case 1:
                X -= Constants.EnemySpeed;
                break;
            case 2:
                Y += Constants.EnemySpeed;
                break;
            case 3:
                Y -= Constants.EnemySpeed;
                break;
        }
    }

    public void Draw(IDisplay display)
    {
        display.DrawCircle(X, Y, 3, Color.Red); // Simplified enemy representation
    }
}