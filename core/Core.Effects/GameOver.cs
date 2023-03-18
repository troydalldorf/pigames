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
        smallFont = new LedFont(LedFontType.Font10x20);
        State = GameOverState.WaitingForUser;
    }
    
    public GameOverState State { get; private set; }

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
        largeFont.DrawText(display, 0, 12, Color.Crimson, "GAME", 0);
        //largeFont.DrawText(display, 5, 32, Color.Crimson, "OVER", 3);
        smallFont.DrawText(display, 0, 48, Color.Blue, "PLAY AGAIN?", 0);
    }
    
    public void Dispose()
    {
        smallFont.Dispose();
    }
}