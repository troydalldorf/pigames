using Core;
using Core.Display.Sprites;
using Core.Effects;

namespace PacificWings.Bits;

public class EnemyWave
{
    private readonly SpriteAnimation enemySprite;
    private List<Enemy> enemies;
    private int waveNumber;

    public EnemyWave(SpriteAnimation enemySprite)
    {
        this.enemySprite = enemySprite;
        this.enemies = new List<Enemy>();
        this.waveNumber = 1;
        this.SpawnWave();
    }

    public void Update(List<Bullet> bullets)
    {
        foreach (var enemy in enemies)
        {
            enemy.Update(bullets);
        }
        enemies.RemoveAll(enemy => enemy.State == Enemy.EnemyState.Destroyed);
        if (enemies.Count == 0)
        {
            waveNumber++;
            SpawnWave();
        }
    }

    public void Draw(IDisplay display)
    {
        foreach (var enemy in enemies)
            enemy.Draw(display);
    }

    private void SpawnWave()
    {
        var enemySpeed = 1 + waveNumber / 5; // Increase enemy speed every 5 waves
        const int enemySpacing = 16;
        const int startX = (64 - enemySpacing * 3) / 2;

        for (var i = 0; i < 4; i++)
        {
            var x = startX + i * enemySpacing;
            var y = 8;
            enemies.Add(new Enemy(x, y, enemySpeed, this.enemySprite));
        }
    }
}