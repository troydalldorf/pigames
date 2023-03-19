namespace Core;

public interface I2PGameElement : IGameElement
{
    void Handle2PInput(IPlayerConsole playerConsole);
}