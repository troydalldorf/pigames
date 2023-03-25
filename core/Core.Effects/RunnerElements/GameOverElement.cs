using System.Drawing;
using Core.Display.Fonts;

namespace Core.Effects.RunnerElements;

public class GameOverElement : IGameElement
{
    private const int Top = 2;
    private int frameCount;
    private readonly LedFont smallFont;
    private readonly LedFont largeFont;
    private Text text = new Text("GAME", "OVER", "PLAY AGAIN?");

    public GameOverElement()
    {
        GameOverAction = GameOverAction.None;
        frameCount = 0;
        smallFont = new LedFont(LedFontType.Font5x8);
        largeFont = new LedFont(LedFontType.Font10x20);
    }
    
    public GameOverAction GameOverAction { get; private set; }
    
    public void HandleInput(IPlayerConsole player1Console)
    {
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

    public GameOverState State()
    {
        return GameOverAction == GameOverAction.None ? GameOverState.None : GameOverState.EndOfGame;
    }

    public void Apply(GameOverState state)
    {
        switch (state)
        {
            case GameOverState.None:
                this.text = new Text("GAME", "OVER", "PLAY AGAIN?");
                break;
            case GameOverState.Draw:
                this.text = new Text(null, "DRAW", "PLAY AGAIN?");
                break;
            case GameOverState.Player1Wins:
                this.text = new Text(null, "P1 WINS", "PLAY AGAIN?");
                break;
            case GameOverState.Player2Wins:
                this.text = new Text(null, "P2 WINS", "PLAY AGAIN?");
                break;
        }
    }

    private record Text(string? Line1, string Line2, string Prompt);
}