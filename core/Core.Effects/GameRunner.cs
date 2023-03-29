using Core.Display;
using Core.Effects.RunnerElements;
using Core.Inputs;

namespace Core.Effects;

public class GameRunner : IDisposable
{
    private int frameCount;
    private readonly LedDisplay display;
    private readonly Player1Console p1Console;
    private readonly Player2Console p2Console;
    private readonly PlayableGameOverElement playableGameOverElement = new PlayableGameOverElement();
    private readonly PauseElement pauseElement = new PauseElement();

    public GameRunner()
    {
        display = new LedDisplay();
        p1Console = new Player1Console();
        p2Console = new Player2Console();
    }

    public void Run(Func<IPlayableGameElement> createGame, int? frameIntervalMs = 33, bool canPause = true, string name = "game")
    {
        Console.WriteLine($"Starting {name}...");
        var leaderBoard = new Leaderboard(name);
        var game = createGame();
        var currentElement = game;
        Console.WriteLine($"Running {name}...");
        var state = GameState.Playing;
        while (state != GameState.Exit)
        {
            // game loop
            currentElement.HandleInput(p1Console);
            if (currentElement is IDuoPlayableGameElement p2GameElement) p2GameElement.Handle2PInput(p2Console);
            currentElement.Update();
            display.Clear();
            currentElement.Draw(display);
            display.Update(frameIntervalMs);

            // play -> GO
            if (currentElement == game && currentElement.State != GameOverState.None)
            {
                playableGameOverElement.Apply(game.State);
                currentElement = playableGameOverElement;
            }
            // play -> pause
            else if (canPause && currentElement == game && p1Console.ReadButtons().IsRedPushed())
            {
                pauseElement.Reset();
                currentElement = pauseElement;
            }
            // GO -> play again
            else if (currentElement == playableGameOverElement && playableGameOverElement.GameOverAction == GameOverAction.PlayAgain)
            {
                DisposeGame(game);
                game = createGame();
                currentElement = game;
            }
            else if (currentElement == playableGameOverElement && playableGameOverElement.GameOverAction == GameOverAction.Exit)
            {
                state = GameState.Exit;
            }
            // pause -> resume
            else if (currentElement == pauseElement && pauseElement.PauseAction == GamePauseAction.Resume)
            {
                currentElement = game;
            }
            // pause -> exit
            else if (currentElement == pauseElement && pauseElement.PauseAction == GamePauseAction.Exit)
            {
                state = GameState.Exit;
            }
        }

        Console.WriteLine($"Exiting {name}...");
        DisposeGame(game);
    }

    private void DisposeGame(IPlayableGameElement? game)
    {
        if (game is IDisposable disposable) disposable.Dispose();
    }

    public void Dispose()
    {
    }
}