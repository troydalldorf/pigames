using System.Drawing;
using Core;
using Core.Display;
using Core.Display.Fonts;
using Core.Inputs;
using Pong;
using Tetris;

namespace All;

public class Menu : IGameElement
{
    private readonly LedFont font = new(LedFontType.FontTomThumb);
    private int cursor = 0;

    private const int Offset = 6;
    private const int ItemHeight = 7;
    private IGameElement? current = null;

    private GameItem[] items =
    {
        new("Pong", null, () => new PongGame()),
        new("Tetris", () => new SoloTetrisGame(), null)
    };

    public void Run(LedDisplay display, PlayerConsole player1Console, PlayerConsole player2Console)
    {
        Console.WriteLine("Starting menu...");
        while (!IsDone())
        {
            if (current != null)
            {
                current.HandleInput(player1Console);
                if (current is I2PGameElement current2P) current2P.Handle2PInput(player2Console);
                current.Update();
                current.Draw(display);
                if (current.IsDone())
                {
                    if (current is IDisposable disposable) disposable.Dispose();
                    current = null;
                }
            }
            else
            {
                this.HandleInput(player1Console);
                this.Update();
                this.Draw(display);
            }
        }
    }

    public void HandleInput(IPlayerConsole console)
    {
        var stick = console.ReadJoystick();
        var buttons = console.ReadButtons();
        if (stick.IsDown()) cursor++;
        if (stick.IsUp()) cursor--;
        if (cursor < 0) cursor = items.Length - 1;
        if (cursor >= items.Length) cursor = 0;

        if (!buttons.IsGreenPushed()) return;
        var item = items[cursor];
        current = item.OnePlayer != null ? item.OnePlayer() : item.TwoPlayer();
    }

    public void Update()
    {
    }

    public void Draw(IDisplay display)
    {
        display.Clear();
        display.DrawRectangle(0, 0, 64, 64, Color.Chartreuse);
        for (var i=0; i < items.Length; i++)
        {
            var item = items[i];
            if (cursor == i)
            {
                display.DrawRectangle(1, 1 + i * ItemHeight-1, 30, ItemHeight, Color.LightSkyBlue, Color.LightSkyBlue);
                font.DrawText(display, 1, Offset + i * ItemHeight, Color.Black, item.Name);
            }
            else
            {
                font.DrawText(display, 1, Offset + i * ItemHeight, Color.LightSkyBlue, item.Name);
            }
        }
        display.Update();
    }

    public bool IsDone()
    {
        return false;
    }
}