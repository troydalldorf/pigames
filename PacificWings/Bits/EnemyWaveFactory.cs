using Core.Display.Sprites;

namespace PacificWings.Bits;

public class EnemyWaveFactory
{
    private static readonly Dictionary<int, EnemyWaveInfo> Waves = new()
    {
        { 1, new EnemyWaveInfo(1, 4, 8, new TopDownMovement()) },
        { 2, new EnemyWaveInfo(1, 4, 8, new RightToLeftMovement()) },
        { 3, new EnemyWaveInfo(1, 4, 8, new LeftToRightMovement()) },
        { 4, new EnemyWaveInfo(1, 4, 8, new CircularMovement()) },
        { 5, new EnemyWaveInfo(2, 5, 8, new TopDownMovement()) },
        { 6, new EnemyWaveInfo(2, 5, 8, new RightToLeftMovement()) },
        { 7, new EnemyWaveInfo(2, 5, 8, new LeftToRightMovement()) },
        { 8, new EnemyWaveInfo(2, 5, 8, new CircularMovement()) },
        { 9, new EnemyWaveInfo(3, 6, 8, new TopDownMovement()) },
        { 10, new EnemyWaveInfo(3, 6, 8, new RightToLeftMovement()) },
        { 11, new EnemyWaveInfo(3, 6, 8, new LeftToRightMovement()) },
        { 12, new EnemyWaveInfo(3, 6, 8, new CircularMovement()) },
    };
    
    public static EnemyWave? CreateWave(int waveNumber, SpriteAnimation enemySprite)
    {
        if (waveNumber > Waves.Count)
            return null;
        var info = Waves[waveNumber];
        var enemyX = 0;
        var enemyY = 0;
        return new EnemyWave(info, enemySprite);
    }
}