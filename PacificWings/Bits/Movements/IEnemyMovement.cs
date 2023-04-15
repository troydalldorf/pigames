using System.Numerics;

namespace PacificWings.Bits.Movements;

public interface IEnemyMovement
{
    Vector2 StartPosition { get; }
    bool Move(Enemy enemy);
}