using Core.Display.Sprites;
using PacificWings.Bits.Movements;

namespace PacificWings.Bits;

public static class EnemyWaveFactory
{
    public static EnemyWave? CreateWave(int waveNumber, SpriteAnimation enemySprite)
    {
        var info = GetWaveInfo(waveNumber, enemySprite);
        return new EnemyWave(info, enemySprite);
    }

    private static EnemyWaveInfo GetWaveInfo(int waveNo, ISprite sprite)
    {
        var speed = waveNo / 10;
        var total = waveNo % 10;
        var spacing = 8 - total;
        var strategy = new TopDownStrategyStrategy(total, spacing, sprite);
        return new EnemyWaveInfo(speed, total, spacing, strategy);
    }
}