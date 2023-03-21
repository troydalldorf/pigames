using System.Diagnostics;
using System.Drawing;
using Breakout;
using Core;
using Core.Display.Fonts;
using Core.Effects;
using FlappyBird;
using Frogger;
using Pong;
using Snake;
using SpaceInvaders2;
using Tetris;

namespace All;

public class Menu : IGameElement
{
    private readonly GameRunner runner;
    private readonly LedFont font = new(LedFontType.FontTomThumb);
    private int cursor;
    private readonly Stopwatch stopwatch = new();
    private long lastActionAt;
    private Random random = new();

    private const int Offset = 6;
    private const int ItemHeight = 6;

    private readonly GameItem[] items =
    {
        new("BREAKOUT", () => new BreakoutGame(), null),
        new("E-PONG", null, () => new PongGame()),
        new("FLAPPY B", () => new FlappyBirdGame(), null),
        new("FROGGER", () => new FroggerGame(), null),
        new("PONG", null, () => new PongGame()),
        new("SNAKE", () => new SnakeGame(), null),
        new("SPACE IN", () => new SpaceInvadersGame(), null),
        new("TETRIS 1", () => new TetrisGame(), null),
        new("TETRIS 2", () => new DuoTetrisGame(), null),
    };

    public Menu(GameRunner runner)
    {
        this.runner = runner;
        stopwatch.Start();
    }

    public void HandleInput(IPlayerConsole console)
    {
        if (stopwatch.ElapsedMilliseconds - lastActionAt < 120)
            return;
        var stick = console.ReadJoystick();
        var buttons = console.ReadButtons();
        if (stick.IsDown()) {
            cursor++;
            lastActionAt = stopwatch.ElapsedMilliseconds;
        }
        if (stick.IsUp())
        {
            cursor--;
            lastActionAt = stopwatch.ElapsedMilliseconds;
        }
        if (cursor < 0) cursor = items.Length - 1;
        if (cursor >= items.Length) cursor = 0;

        if (buttons.IsGreenPushed())
        {
            var item = items[cursor];
            var game = item.OnePlayer ?? item.TwoPlayer;
            runner.Run(game);
        }

        var r = random.Next(0, 4);
        console.LightButtons(r == 0, r == 2, r == 3, r == 4);
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