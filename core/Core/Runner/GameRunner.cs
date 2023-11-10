using Core.Display;
using Core.Fonts;
using Core.Inputs;
using Core.Runner.RunnerElements;
using Core.Runner.State;
using Core.Scores;
using Core.Sounds;

namespace Core.Runner;

public class GameRunner : IDisposable
{
    private readonly IDisplay display;
    private readonly IFontFactory fontFactory;
    private readonly Player1Console p1Console;
    private readonly Player2Console p2Console;
    private readonly PlayableGameOverElement gameOverElement;
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
        this.gameOverElement = new PlayableGameOverElement(fontFactory);
    }

    public void Run(Func<IPlayableGameElement> createGame, int? frameIntervalMs = 33, bool canPause = true, string name = "game")
    {
        // TODO: Crete new game when replaying - dispose and create new
        Console.WriteLine($"Starting {name}...");
        var playing = new RunnerState("playing", createGame(), GameState.Playing);
        var paused = new RunnerState("paused", pauseElement, GameState.Playing, pauseElement.Reset);
        var gameOver = new RunnerState("game-over", gameOverElement, GameState.Playing, () => gameOverElement.Apply(playing.Element.State));
        var exit = new RunnerState("exit", gameOverElement, GameState.Exit);
        var leaderboard = new RunnerState("leaderboard", new Leaderboard(name, this.fontFactory), GameState.Playing);
        
        playing = playing
            .AddTransition(gameOver, ge => ge.State != GameOverState.None)
            .AddTransition(paused, _ => canPause && p1Console.ReadButtons().IsYellowPushed());
        gameOver = gameOver
            .AddTransition(playing, ge => gameOverElement.GameOverAction == GameOverAction.PlayAgain)
            .AddTransition(exit, _ => gameOverElement.GameOverAction == GameOverAction.Exit);
        paused = paused
            .AddTransition(playing, _ => pauseElement.PauseAction == GamePauseAction.Resume)
            .AddTransition(exit, _ => pauseElement.PauseAction == GamePauseAction.Exit);
        
        Console.WriteLine($"Running {name}...");
        var current = playing;
        Console.WriteLine($"Running {name} with initial state, {current.Name}...");
        while (current.State != GameState.Exit)
        {
            current.Element.HandleInput(p1Console);
            if (current.Element is IDuoPlayableGameElement p2GameElement)
                p2GameElement.Handle2PInput(p2Console);
            current.Element.Update();
            display.Clear();
            current.Element.Draw(display);
            display.Update(frameIntervalMs);

            current = current.TryTransition();
        }

        Console.WriteLine($"Exiting {name}...");
        DisposeGame(playing.Element);
    }

    private void DisposeGame(IPlayableGameElement? game)
    {
        if (game is IDisposable disposable)
            disposable.Dispose();
    }

    public void Dispose()
    {
    }
}