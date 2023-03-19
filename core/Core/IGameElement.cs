namespace Core;

public interface IGameElement
{
    void HandleInput(IPlayerConsole player1Console);
    void Update();
    void Draw(IDisplay display);
    bool IsDone();
}