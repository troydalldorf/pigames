using System.Numerics;

namespace PacificWings.Bits.Movements;

public interface IMovementStrategy
{
    Vector2 StartPosition { get; }
    bool Move(Enemy enemy);
}