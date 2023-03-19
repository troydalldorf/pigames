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
    private readonly int cursor = 0;

    private const int Offset = 5;
    private const int ItemHeight = 7;

    private GameItem[] items =
    {
        new("Pong", null, () => new PongGame()),
        new("Tetris", () => new SoloTetrisGame(), null)
    };

    public void Run(LedDisplay display, PlayerConsole player1Console, PlayerConsole player2Console)
    {
        Console.WriteLine("Starting menu...");
        while (true)
        {
            this.HandleInput(player1Console);
            this.Update();
            this.Draw(display);
        }
    }

    public void HandleInput(IPlayerConsole console)
    {
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
            display.DrawCircle(3 + ItemHeight/2, Offset + i*ItemHeight + ItemHeight/2, ItemHeight/2-1, i==cursor ?  Color.Red : Color.LightSkyBlue);
            font.DrawText(display, 3 + ItemHeight + 1, Offset + i*ItemHeight, Color.LightSkyBlue, item.Name);
        }
        display.Update();
    }
}