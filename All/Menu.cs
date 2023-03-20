using System.Drawing;
using Breakout;
using Core;
using Core.Display.Fonts;
using Core.Effects;
using Pong;
using Snake;
using SpaceInvaders2;
using Tetris;

namespace All;

public class Menu : IGameElement
{
    private readonly GameRunner runner;
    private readonly LedFont font = new(LedFontType.FontTomThumb);
    private int cursor = 0;

    private const int Offset = 6;
    private const int ItemHeight = 7;

    private GameItem[] items =
    {
        new("BREAKOUT", () => new BreakoutGame(), null),
        new("E-PONG", null, () => new PongGame()),
        new("PONG", null, () => new PongGame()),
        new("SNAKE", () => new SnakeGame(), null),
        new("SPACE INV.", () => new SpaceInvadersGame(), null),
        new("TETRIS 1P", () => new TetrisGame(), null),
        new("TETRIS 2P", () => new DuoTetrisGame(), null),
    };

    public Menu(GameRunner runner)
    {
        this.runner = runner;
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
        var game = item.OnePlayer != null ? item.OnePlayer() : item.TwoPlayer();
        runner.Run(game);
    }

    public void Update()
    {
    }

    public void Draw(IDisplay display)
    {
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
    }

    public bool IsDone()
    {
        return false;
    }
}