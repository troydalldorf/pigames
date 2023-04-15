using Core.Display.Sprites;

namespace PacificWings.Bits.Movements;

public interface IMovementStrategyFactory
{
    IMovementStrategy Create(int no);
}