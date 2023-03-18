using System.Drawing;
using Core.Display;
using Core.Display.Fonts;
using Core.Inputs;

namespace Core.Effects;

public class GameOver : IGameElement, IDisposable
{
    private LedFont largeFont;
    private LedFont smallFont;
    
    public GameOver()
    {
        largeFont = new LedFont(LedFontType.Font10x20);
        smallFont = new LedFont(LedFontType.Font5x8);
        State = GameOverState.WaitingForUser;
    }
    
    public GameOverState State { get;  set; }

    public void Update(PlayerConsole console)
    {
        var buttons = console.ReadButtons();
        if (buttons.IsGreenPushed())
            State = GameOverState.PlayAgain;
        else if (buttons.IsRedPushed())
            State = GameOverState.Done;
    }

    public void Draw(LedDisplay display)
    {
        largeFont.DrawText(display, 7, 14, Color.Crimson, "GAME", 3);
        largeFont.DrawText(display, 7, 29, Color.Crimson, "OVER", 3);
        display.DrawRectangle(0, 38, 64, 8, Color.Black, Color.Black);
        smallFont.DrawText(display, 2, 38, Color.Blue, "PLAY AGAIN?", 0);
    }
    
    public void Dispose()
    {
        smallFont.Dispose();
    }
}