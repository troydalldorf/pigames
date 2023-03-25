namespace Core;

public interface IDuoGameElement : IGameElement
{
    void Handle2PInput(IPlayerConsole player2Console);
}