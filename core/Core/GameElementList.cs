using Core.Display;

namespace Core;

public class GameElementList : IDuoPlayableGameElement
{
    private readonly List<IGameElement> gameElements = new List<IGameElement>();

    public void Add(IGameElement gameElement)
    {
        gameElements.Add(gameElement);
    }

    public void HandleInput(IPlayerConsole playerConsole)
    {
        foreach (var gameElement in gameElements)
        {
            if (gameElement is IPlayableGameElement playableGameElement)
            {
                playableGameElement.HandleInput(playerConsole);
            }
        }
    }

    public void Handle2PInput(IPlayerConsole playerConsole)
    {
        foreach (var gameElement in gameElements)
        {
            if (gameElement is IDuoPlayableGameElement playableGameElement)
            {
                playableGameElement.Handle2PInput(playerConsole);
            }
        }
    }

    public void Update()
    {
        foreach (var gameElement in gameElements)
        {
            gameElement.Update();
        }
    }

    public void Draw(IDisplay display)
    {
        foreach (var gameElement in gameElements)
        {
            gameElement.Draw(display);
        }
    }

    public GameOverState State
    {
        get
        {
            return gameElements.OfType<IPlayableGameElement>().Any(x => x.State != GameOverState.None)
                ? GameOverState.EndOfGame
                : GameOverState.None;
        }
    }
}