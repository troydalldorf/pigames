using System.Numerics;

namespace PacificWings.Bits.Movements;

public interface IMovementStrategy
{
    Point StartPosition { get; }
    bool Move(Enemy enemy);
}