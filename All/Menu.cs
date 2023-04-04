using System.Diagnostics;
using System.Drawing;
using BomberMan;
using Breakout;
using Checkers;
using ConnectFour;
using Core;
using Core.Fonts;
using Core.Runner;
using FlappyBird;
using Frogger;
using MemoryCard;
using Minesweeper;
using Othello;
using Pong;
using Snake;
using SpaceInvaders2;
using Tetris;

namespace All;

public class Menu : IPlayableGameElement
{
    private readonly GameRunner runner;
    private readonly IFontFactory fontFactory;
    private readonly IFont font;
    private int cursor;
    private readonly Stopwatch stopwatch = new();
    private long lastActionAt;
    private readonly Random random = new();

    private const int Offset = 6;
    private const int ItemHeight = 6;

    private readonly GameItem[] items;

    public Menu(GameRunner runner, IFontFactory fontFactory)
    {
        this.fontFactory = fontFactory;
        this.runner = runner;
        this.font = fontFactory.GetFont(LedFontType.FontTomThumb);
        stopwatch.Start();
        this.items = new GameItem[]
        {
            new("Asteroid", () => new AsteroidsGame()),
            new("B Man", () => new BombermanGame(fontFactory)),
            new("Breakout", () => new BreakoutGame()),
            new("C-Four", () => new ConnectFourGame(), 100),
            new("Checkers", () => new CheckersGame(fontFactory)),
            new("E-Pong", () => new PongPlayableGame(fontFactory)),
            new("Flappy B", () => new FlappyBirdPlayableGame(fontFactory), 50),
            new("Frogger", () => new FroggerPlayableGame(), 100),
            new("Othello", () => new OthelloGame(fontFactory)),
            new("Memory", () => new MemoryCardGame()),
            new("Mines", () => new MinesweeperGame(fontFactory)),
            new("Pong", () => new PongPlayableGame(fontFactory)),
            new("Snake", () => new SnakePlayableGame(), 100),
            new("Snake 2", () => new SnakeGame2P(), 100),
            new("Space I", () => new SpaceInvadersPlayableGame(), 75),
            new("Tetris 1", () => new TetrisGame(fontFactory)),
            new("Tetris 2", () => new DuoPlayableTetrisGame(fontFactory)),
        };
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
            var (name, game, displayInterval) = items[cursor];
            runner.Run(game, displayInterval, name:name);
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