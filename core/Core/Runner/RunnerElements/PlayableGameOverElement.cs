using System.Diagnostics;
using System.Drawing;
using Core.Display;
using Core.Fonts;
using Core.Inputs;
using Core.State;

namespace Core.Runner.RunnerElements;

public class PlayableGameOverElement : IPlayableGameElement
{
    private const int Top = 2;
    private int frameCount;
    private readonly IFont smallFont;
    private readonly IFont largeFont;
    private Text text = new Text("GAME", "OVER", "PLAY AGAIN?");
    private readonly Stopwatch stopwatch = new Stopwatch();

    public PlayableGameOverElement(IFontFactory fontFactory)
    {
        GameOverAction = GameOverAction.None;
        frameCount = 0;
        smallFont = fontFactory.GetFont(LedFontType.Font5x8);
        largeFont = fontFactory.GetFont(LedFontType.Font10x20);
    }
    
    public GameOverAction GameOverAction { get; private set; }
    
    public void HandleInput(IPlayerConsole player1Console)
    {
        if (stopwatch.ElapsedMilliseconds < 300) return;
        
        var buttons = player1Console.ReadButtons();
        if (buttons.IsGreenPushed())
            GameOverAction = GameOverAction.PlayAgain;
        else if (buttons.IsRedPushed())
            GameOverAction = GameOverAction.Exit;

        // LED Buttons
        frameCount++;
        var button = frameCount % 6 < 3;
        player1Console.LightButtons(button, !button, false, false);
    }

    public void Update()
    {
    }

    public void Draw(IDisplay display)
    {
        if (text.Line1 != null)
        {
            largeFont.DrawText(display, 6, Top + 12, Color.Black, text.Line1, 3);
            largeFont.DrawText(display, 8, Top + 14, Color.Black, text.Line1, 3);
            largeFont.DrawText(display, 7, Top + 13, Color.Crimson, text.Line1, 3);
        }

        largeFont.DrawText(display, 6, Top + 27, Color.Black, text.Line2, 3);
        largeFont.DrawText(display, 8, Top + 29, Color.Black, text.Line2, 3);
        largeFont.DrawText(display, 7, Top + 28, Color.Crimson, text.Line2, 3);
        display.DrawRectangle(0, Top + 36 - 8, 64, 8, Color.Black, Color.Black);
        smallFont.DrawText(display, 4, Top + 36, Color.Blue, text.Prompt, 0);
    }

    public GameOverState State => GameOverAction == GameOverAction.None ? GameOverState.None : GameOverState.EndOfGame;

    public void Apply(GameOverState state)
    {
        stopwatch.Restart();
        GameOverAction = GameOverAction.None;
        this.text = state switch
        {
            GameOverState.None => new Text("GAME", "OVER", "PLAY AGAIN?"),
            GameOverState.Draw => new Text(null, "DRAW", "PLAY AGAIN?"),
            GameOverState.Player1Wins => new Text(" P1", "WINS", "PLAY AGAIN?"),
            GameOverState.Player2Wins => new Text(" P2", "WINS", "PLAY AGAIN?"),
            GameOverState.EndOfGame => new Text("P1", "LOSE", "PLAY AGAIN?"),
            _ => this.text
        };
    }

    private record Text(string? Line1, string Line2, string Prompt);
}