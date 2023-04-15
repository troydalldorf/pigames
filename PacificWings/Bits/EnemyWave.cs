using Core;
using Core.Display.Sprites;
using Core.Effects;

namespace PacificWings.Bits;

public class EnemyWave
{
    private readonly SpriteAnimation enemySprite;
    private List<Enemy> enemies;
    public bool IsComplete => enemies.Count == 0;

    public EnemyWave(EnemyWaveInfo info, SpriteAnimation enemySprite)
    {
        this.enemySprite = enemySprite;
        this.enemies = new List<Enemy>();
        this.SpawnWave(info);
    }

    public void Update(List<Bullet> bullets)
    {
        foreach (var enemy in enemies)
        {
            enemy.Update(bullets);
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
        for (var i = 0; i < 4; i++)
        {
            var (x, y) = info.MovementStrategy.Start(i, this.enemySprite.Width, this.enemySprite.Height, info.EnemySpacing);
            enemies.Add(new Enemy(x, y, info.Speed, info.MovementStrategy, this.enemySprite));
        }
    }
}