namespace PacificWings.Bits;

public record EnemyWaveInfo(int Speed, int EnemyCount, int EnemySpacing, IEnemyMovement MovementStrategy);
