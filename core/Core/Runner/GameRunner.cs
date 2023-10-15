using Core.Display;
using Core.Fonts;
using Core.Inputs;
using Core.Runner.RunnerElements;
using Core.Sounds;

namespace Core.Runner;

public class GameRunner : IDisposable
{
    private readonly IDisplay display;
    private readonly IFontFactory fontFactory;
    private readonly Player1Console p1Console;
    private readonly Player2Console p2Console;
    private readonly PlayableGameOverElement playableGameOverElement;
    private readonly PauseElement pauseElement;
    //private readonly SoundPlayer player = new SoundPlayer();
    // private readonly Sound winSound = new Sound("./sfx/win.mp3");
    // private readonly Sound gameOverSound = new Sound("./sfx/game-over.mp3");

    public GameRunner(IDisplay display, IFontFactory fontFactory)
    {
        this.display = display;
        this.fontFactory = fontFactory;
        p1Console = new Player1Console();
        p2Console = new Player2Console();
        this.pauseElement = new PauseElement(fontFactory);
        this.playableGameOverElement = new PlayableGameOverElement(fontFactory);
    }

    public void Run(Func<IPlayableGameElement> createGame, int? frameIntervalMs = 33, bool canPause = true, string name = "game")
    {
        Console.WriteLine($"Starting {name}...");
        var leaderBoard = new Leaderboard(name, this.fontFactory);
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
                if (game.State is GameOverState.Player1Wins or GameOverState.Player2Wins or GameOverState.Draw)
                {
                    // player.Play(winSound);
                }
                else
                {
                    // player.Play(gameOverSound);
                }
            }
            // play -> pause
            else if (canPause && currentElement == game && p1Console.ReadButtons().IsYellowPushed())
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