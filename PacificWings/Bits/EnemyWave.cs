using Core;
using Core.Display.Sprites;

namespace PacificWings.Bits;

public class EnemyWave
{
    private readonly SpriteAnimation enemySprite;
    private readonly List<Enemy> enemies;
    public bool IsComplete => enemies.Count == 0;

    public EnemyWave(EnemyWaveInfo info, SpriteAnimation enemySprite)
    {
        this.enemySprite = enemySprite;
        this.enemies = new List<Enemy>();
        this.SpawnWave(info);
    }

    public void Update(List<Bullet> bullets, Player player)
    {
        foreach (var enemy in enemies)
        {
            enemy.Update(bullets, player);
        }
        enemies.RemoveAll(enemy => enemy.State == Enemy.EnemyState.Destroyed);
    }

    public void Draw(IDisplay display)
    {
        foreach (var enemy in enemies)
            enemy.Draw(display);
    }

    private void SpawnWave(EnemyWaveInfo info)
    {
         var startX = (64 - info.EnemySpacing * 3) / 2;
        for (var i = 0; i < info.EnemyCount; i++)
        {
            var strategy = info.MovementStrategyFactory.Create(i);
            enemies.Add(new Enemy((int)strategy.StartPosition.X, (int)strategy.StartPosition.Y, info.Speed, strategy, this.enemySprite));
        }
    }
}