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
        var speed = Math.Clamp(1 + waveNo / 10, 1, 4);
        var total = 4 + waveNo % 10;
        var spacing = 8 - total;
        IMovementStrategyFactory strategy = (waveNo % 4) switch
        {
            0 => new TopDown(total, spacing, sprite),
            1 => new TopLeftToBottomRight(total, spacing, speed, sprite),
            2 => new TopRightToBottomLeft(total, spacing, speed, sprite),
            3 => new Loop(total, spacing, speed, sprite),
            _ => throw new NotImplementedException($"No strategy for {waveNo % 3}")
        };
        return new EnemyWaveInfo(speed, total, spacing, strategy);
    }
}