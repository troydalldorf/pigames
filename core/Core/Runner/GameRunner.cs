using Core.Display;
using Core.Fonts;
using Core.Inputs;
using Core.Runner.RunnerElements;
using Core.Runner.State;
using Core.State;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Runner;

public class GameRunner : IDisposable, IGameRunner
{
    private readonly IServiceScopeFactory scopeFactory;
    private readonly IDisplay display;
    private readonly IFontFactory fontFactory;
    private readonly IPlayerConsole p1Console;
    private readonly IPlayerConsole p2Console;
    private readonly PlayableGameOverElement gameOverElement;
    private readonly PauseElement pauseElement;
    //private readonly SoundPlayer player = new SoundPlayer();
    // private readonly Sound winSound = new Sound("./sfx/win.mp3");
    // private readonly Sound gameOverSound = new Sound("./sfx/game-over.mp3");

    public GameRunner(
        IServiceScopeFactory scopeFactory, IDisplay display, IPlayerConsole player1, IPlayerConsole player2, IFontFactory fontFactory)
    {
        this.scopeFactory = scopeFactory;
        this.display = display;
        this.fontFactory = fontFactory;
        p1Console = player1;
        p2Console = player2;
        this.pauseElement = new PauseElement(fontFactory);
        this.gameOverElement = new PlayableGameOverElement(fontFactory);
    }

    public void Run(Func<IPlayableGameElement> createGame, GameRunnerOptions options)
    {
        Console.WriteLine($"Starting {options.Name}...");
        var playing = new RunnerState("playing", createGame(), GameState.Playing);
        var paused = new RunnerState("paused", pauseElement, GameState.Playing, pauseElement.Reset);
        var gameOver = new RunnerState("game-over", gameOverElement, GameState.Playing, () => gameOverElement.Apply(playing.Element.State));
        var exit = new RunnerState("exit", gameOverElement, GameState.Exit);
        var leaderboard = new RunnerState("leaderboard", new Leaderboard(options.Name, this.fontFactory), GameState.Playing);
        
        playing
            .AddTransition(gameOver, ge => ge.State != GameOverState.None)
            .AddTransition(paused, _ => options.CanPause && p1Console.ReadButtons().IsYellowPushed());
        gameOver
            .AddTransition(playing, ge => gameOverElement.GameOverAction == GameOverAction.PlayAgain)
            .AddTransition(exit, _ => gameOverElement.GameOverAction == GameOverAction.Exit);
        paused
            .AddTransition(playing, _ => pauseElement.PauseAction == GamePauseAction.Resume)
            .AddTransition(exit, _ => pauseElement.PauseAction == GamePauseAction.Exit);
        
        Console.WriteLine($"Running {options.Name}...");
        var current = playing;
        Console.WriteLine($"Running {options.Name} with initial state, {current.Name}...");
        while (current.State != GameState.Exit)
        {
            current.Element.HandleInput(p1Console);
            if (current.Element is IDuoPlayableGameElement p2GameElement)
                p2GameElement.Handle2PInput(p2Console);
            current.Element.Update();
            display.Clear();
            current.Element.Draw(display);
            display.Update(options.FrameIntervalMs);

            current = current.TryTransition();
        }

        Console.WriteLine($"Exiting {options.Name}...");
        DisposeGame(playing.Element);
    }

    public void Run<TGame>(GameRunnerOptions options)
        where TGame : IPlayableGameElement
    {
        using var scope = this.scopeFactory.CreateScope();
        Run(() => scope.ServiceProvider.GetRequiredService<TGame>(), options);
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