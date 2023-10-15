using System.Diagnostics;
using System.Drawing;
using Core.Display;
using Core.Fonts;

namespace Core.Runner.RunnerElements;

public class PauseElement : IPlayableGameElement
{
    private readonly IFontFactory fontFactory;
    private const int Top = 2;
    private int frameCount;
    private readonly IFont smallFont;
    private readonly IFont largeFont;
    private Stopwatch stopwatch = new Stopwatch();
    
    public PauseElement(IFontFactory fontFactory)
    {
        this.fontFactory = fontFactory;
        PauseAction = GamePauseAction.Paused;
        frameCount = 0;
        smallFont = fontFactory.GetFont(LedFontType.Font5x8);
        largeFont = fontFactory.GetFont(LedFontType.Font5x8);
    }
    
    public GamePauseAction PauseAction { get; private set; }
    
    public void HandleInput(IPlayerConsole player1Console)
    {
        if (stopwatch.ElapsedMilliseconds < 300) return;

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

    public GameOverState State => PauseAction == GamePauseAction.Paused ? GameOverState.None : GameOverState.EndOfGame;

    public void Reset()
    {
        stopwatch.Restart();
        PauseAction = GamePauseAction.Paused;
    }
}