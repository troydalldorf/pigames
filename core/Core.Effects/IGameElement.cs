using Core.Inputs;

namespace Core.Display;

public interface IGameElement
{
    void Update(PlayerConsole console);
    void Draw(LedDisplay display);
}