using System.Drawing;
using Core.Display;
using Core.Display.Fonts;
using Core.Inputs;

namespace Core.Effects;

public class GameRunner : IDisposable
{
    private readonly int top;
    private readonly LedFont largeFont;
    private readonly LedFont smallFont;
    private int frameCount;
    private GameState state;
    private readonly LedDisplay display;
    private readonly Player1Console p1Console;
    private readonly Player2Console p2Console;
    
    public GameRunner()
    {
        top = 2;
        largeFont = new LedFont(LedFontType.Font10x20);
        smallFont = new LedFont(LedFontType.Font5x8);
        display = new LedDisplay(); 
        p1Console = new Player1Console();
        p2Console = new Player2Console();
        state = GameState.WaitingToStart;
    }
    
    public void Run(IGameElement game, int? frameIntervalMs = 33)
    {
        state = GameState.Running;
        while (state != GameState.Done)
        {
            switch (state)
            {
                case GameState.Running:
                {
                    game.HandleInput(p1Console);
                    if (game is I2PGameElement p2GameElement) p2GameElement.Handle2PInput(p2Console);
                    break;
                }
                case GameState.GameOver:
                    HandleGameOverInput(p1Console);
                    break;
            }
            game.Update();
            display.Clear();
            game.Draw(display);
            switch (state)
            {
                case GameState.GameOver:
                    DrawGameOver();
                    break;
            }
            display.Update(frameIntervalMs);
        }
    }

    private void HandleGameOverInput(IPlayerConsole console)
    {
        var buttons = console.ReadButtons();
        if (buttons.IsGreenPushed())
            state = GameState.PlayAgain;
        else if (buttons.IsRedPushed())
            state = GameState.Done;
        
        // LED Buttons
        frameCount++;
        var button = frameCount % 6 < 3;
        console.LightButtons(button, !button, false, false);
    }

    public void DrawGameOver()
    {
        largeFont.DrawText(display, 7, top+13, Color.Crimson, "GAME", 3);
        largeFont.DrawText(display, 7, top+28, Color.Crimson, "OVER", 3);
        display.DrawRectangle(0, top+36, 64, 8, Color.Black, Color.Black);
        smallFont.DrawText(display, 4, top+36, Color.Blue, "PLAY AGAIN?", 0);
    }

    public void Dispose()
    {
        smallFont.Dispose();
        largeFont.Dispose();
    }
}