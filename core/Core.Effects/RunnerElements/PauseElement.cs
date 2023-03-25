using System.Drawing;
using Core.Display.Fonts;

namespace Core.Effects.RunnerElements;

public class PauseElement : IGameElement
{
    private const int Top = 2;
    private int frameCount;
    private readonly LedFont smallFont;
    private readonly LedFont largeFont;
    
    public PauseElement()
    {
        PauseAction = GamePauseAction.Paused;
        frameCount = 0;
        smallFont = new LedFont(LedFontType.Font5x8);
        largeFont = new LedFont(LedFontType.Font10x20);
    }
    
    public GamePauseAction PauseAction { get; private set; }
    
    public void HandleInput(IPlayerConsole player1Console)
    {
        var buttons = player1Console.ReadButtons();
        if (buttons.IsGreenPushed())
            PauseAction = GamePauseAction.Resume;
        else if (buttons.IsRedPushed())
            PauseAction = GamePauseAction.Exit;

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
        largeFont.DrawText(display, 0, Top + 27, Color.Black, "PAUSED", 3);
        largeFont.DrawText(display, 2, Top + 29, Color.Black, "PAUSED", 3);
        largeFont.DrawText(display, 1, Top + 28, Color.Crimson, "PAUSED", 3);
        display.DrawRectangle(0, Top + 36 - 8, 64, 8, Color.Black, Color.Black);
        smallFont.DrawText(display, 4, Top + 36, Color.Blue, "CONTINUE?", 0);
    }

    public GameOverState State()
    {
        return PauseAction != GamePauseAction.Paused ? GameOverState.EndOfGame : GameOverState.None;
    }

    public void Reset()
    {
        PauseAction = GamePauseAction.Paused;
    }
}