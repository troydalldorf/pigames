using PacificWings.Bits.Movements;

namespace PacificWings.Bits;

public record EnemyWaveInfo(int Speed, int EnemyCount, int EnemySpacing, IMovementStrategyFactory MovementStrategyFactory);
