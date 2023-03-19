namespace Core;

public interface IGameElement
{
    void HandleInput(IPlayerConsole console);
    void Update();
    void Draw(IDisplay display);
}