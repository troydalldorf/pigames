using Core.Display.Sprites;
using PacificWings.Bits.Movements;

namespace PacificWings.Bits;

public static class EnemyWaveFactory
{
    public static EnemyWave? CreateWave(int waveNumber, SpriteAnimation enemySprite)
    {
        var info = GetWaveInfo(waveNumber, enemySprite.Height);
        return new EnemyWave(info, enemySprite);
    }

    private static EnemyWaveInfo GetWaveInfo(int waveNo, int enemyHeight)
    {
        var speed = waveNo / 10;
        var count = waveNo % 10;
        var spacing = 8 - count;
        var strategy = () => new TopDownStrategy(8, enemyHeight);
        return new EnemyWaveInfo(speed, count, spacing, strategy);
    }
}