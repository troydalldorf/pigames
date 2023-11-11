using Core.Display;
using Core.Inputs;
using Core.State;

namespace Core;

public interface IGameElement
{
    void Update();
    void Draw(IDisplay display);
}

public interface IPlayableGameElement : IGameElement
{
    void HandleInput(IPlayerConsole player1Console);
    GameOverState State { get; }
}