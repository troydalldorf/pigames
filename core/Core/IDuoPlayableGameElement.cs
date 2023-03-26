namespace Core;

public interface IDuoPlayableGameElement : IPlayableGameElement
{
    void Handle2PInput(IPlayerConsole player2Console);
}