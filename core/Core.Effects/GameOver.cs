using System.Drawing;
using Core.Display;
using Core.Display.Fonts;
using Core.Inputs;

namespace Core.Effects;

public class GameOver : IGameElement, IDisposable
{
    private LedFont largeFont;
    private LedFont smallFont;
    private int frameCount;
    
    public GameOver()
    {
        largeFont = new LedFont(LedFontType.Font10x20);
        smallFont = new LedFont(LedFontType.Font5x8);
        State = GameState.GameOver;
    }
    
    public GameState State { get;  set; }

    public void Update(PlayerConsole console)
    {
        frameCount++;
        var buttons = console.ReadButtons();
        if (buttons.IsGreenPushed())
            State = GameState.PlayAgain;
        else if (buttons.IsRedPushed())
            State = GameState.Done;
        var button = frameCount % 6 < 3;
        console.LightButtons(button, !button, false, false);
    }

    public void Draw(LedDisplay display)
    {
        largeFont.DrawText(display, 7, 15, Color.Crimson, "GAME", 3);
        largeFont.DrawText(display, 7, 30, Color.Crimson, "OVER", 3);
        display.DrawRectangle(0, 38, 64, 8, Color.Black, Color.Black);
        smallFont.DrawText(display, 4, 38, Color.Blue, "PLAY AGAIN?", 0);
    }
    
    public void Dispose()
    {
        smallFont.Dispose();
    }
}