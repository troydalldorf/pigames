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
    }
    
    public void Run(Func<IGameElement> createGame, int? frameIntervalMs = 33)
    {
        Console.WriteLine("Starting game...");
        var game = createGame();
        Console.WriteLine("Running game...");
        var state = GameState.Running;
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
                    HandleGameOverInput(p1Console, ref state);
                    break;
                case GameState.PlayAgain:
                    DisposeGame(game);
                    game = createGame();
                    state = GameState.Running;
                    break;
            }
            game.Update();
            if (state == GameState.Running && game.IsDone()) state = GameState.GameOver;
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
        Console.WriteLine("Exiting game...");
        DisposeGame(game);
    }

    private void HandleGameOverInput(IPlayerConsole console, ref GameState state)
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

    private void DrawGameOver()
    {
        largeFont.DrawText(display, 6, top+12, Color.Black, "GAME", 3);
        largeFont.DrawText(display, 8, top+14, Color.Black, "GAME", 3);
        largeFont.DrawText(display, 7, top+13, Color.Crimson, "GAME", 3);
        largeFont.DrawText(display, 6, top+27, Color.Black, "OVER", 3);
        largeFont.DrawText(display, 8, top+29, Color.Black, "OVER", 3);
        largeFont.DrawText(display, 7, top+28, Color.Crimson, "OVER", 3);
        display.DrawRectangle(0, top+36-8, 64, 8, Color.Black, Color.Black);
        smallFont.DrawText(display, 4, top+36, Color.Blue, "PLAY AGAIN?", 0);
    }

    private void DisposeGame(IGameElement? game)
    {
        if (game is IDisposable disposable) disposable.Dispose();
    }
    
    public void Dispose()
    {
        smallFont.Dispose();
        largeFont.Dispose();
    }
}