using Core;
using Core.Display.Sprites;

namespace PacificWings.Bits;

public class EnemyWave
{
    private readonly SpriteAnimation enemySprite;
    private List<Enemy> enemies;
    private int waveNumber;

    public EnemyWave(SpriteAnimation enemySprite)
    {
        this.enemySprite = enemySprite;
        enemies = new List<Enemy>();
        waveNumber = 1;
        SpawnWave();
    }

    public void Update(List<Bullet> bullets)
    {
        foreach (var enemy in enemies)
        {
            enemy.Update(bullets);
        }

        // Remove destroyed enemies
        enemies.RemoveAll(enemy => enemy.IsDestroyed);

        // Check if wave is cleared
        if (enemies.Count == 0)
        {
            waveNumber++;
            SpawnWave();
        }
    }

    public void Draw(IDisplay display)
    {
        foreach (var enemy in enemies)
        {
            enemy.Draw(display);
        }
    }

    private void SpawnWave()
    {
        var enemySpeed = 1 + waveNumber / 5; // Increase enemy speed every 5 waves
        var enemySpacing = 16;
        var startX = (64 - enemySpacing * 3) / 2;

        for (var i = 0; i < 4; i++)
        {
            var x = startX + i * enemySpacing;
            var y = 8;
            enemies.Add(new Enemy(x, y, enemySpeed, this.enemySprite));
        }
    }
}