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
    private readonly LedDisplay display;
    private readonly PlayerConsole player1Console;
    private readonly PlayerConsole player2Console;
    private readonly LedFont font = new(LedFontType.Font4x6);
    private readonly int cursor = 0;

    private const int Offset = 5;
    private const int ItemHeight = 7;

    private GameItem[] items =
    {
        new("Pong", null, () => new PongGame()),
        new("Tetris", () => new SoloTetrisGame(), null)
    };

    public Menu(LedDisplay display, PlayerConsole player1Console, PlayerConsole player2Console)
    {
        this.display = display;
        this.player1Console = player1Console;
        this.player2Console = player2Console;
    }

    public void Run()
    {
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
        var y = 10;
        for (var i=0; i< items.Length; i++)
        {
            var item = items[i];
            display.DrawCircle(3 + ItemHeight/2, Offset + ItemHeight/2, ItemHeight/2-1, i==cursor ?  Color.Red : Color.LightSkyBlue);
            font.DrawText(display, 5, Offset + i*ItemHeight, Color.LightSkyBlue, item.Name);
            y += 7;
        }
    }
}