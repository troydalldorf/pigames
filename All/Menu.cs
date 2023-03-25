using System.Diagnostics;
using System.Drawing;
using BomberMan;
using Breakout;
using Core;
using Core.Display.Fonts;
using Core.Effects;
using FlappyBird;
using Frogger;
using Othello;
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
    private readonly Random random = new();

    private const int Offset = 6;
    private const int ItemHeight = 6;

    private readonly GameItem[] items =
    {
        new("B MAN", () => new BombermanGame()),
        new("BREAKOUT", () => new BreakoutGame()),
        new("E-PONG", () => new PongGame()),
        new("FLAPPY B", () => new FlappyBirdGame()),
        new("FROGGER", () => new FroggerGame()),
        new("OTHELLO", () => new OthelloGame()),
        new("PONG", () => new PongGame()),
        new("SNAKE", () => new SnakeGame()),
        new("SPACE IN", () => new SpaceInvadersGame()),
        new("TETRIS 1", () => new TetrisGame()),
        new("TETRIS 2", () => new DuoTetrisGame()),
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
            var game = item.CreateGame;
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
        display.DrawRectangle(0, 0, 64, 64, Color.Blue);
        for (var i=0; i < items.Length; i++)
        {
            var item = items[i];
            var x = 1 + i / 10 * 32;
            var y = Offset + i % 10 * ItemHeight;
            if (cursor == i)
            {
                display.DrawRectangle(x, y-5, 30, ItemHeight, Color.LightSkyBlue, Color.LightSkyBlue);
                font.DrawText(display, x, y, Color.Black, item.Name);
            }
            else
            {
                font.DrawText(display, x, y, Color.LightSkyBlue, item.Name);
            }
        }
    }

    public GameOverState State => GameOverState.None;
}