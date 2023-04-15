using System.Diagnostics;
using System.Drawing;
using Asteroids;
using BomberMan;
using Breakout;
using Checkers;
using Chess;
using ConnectFour;
using Core;
using Core.Fonts;
using Core.Runner;
using Core.Sounds;
using FlappyBird;
using Frogger;
using MemoryCard;
using Minesweeper;
using Othello;
using PacificWings;
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
    private readonly SoundPlayer soundPlayer = new();
    private readonly Sound selectSound = new Sound("./sfx/select.mp3");

    private const int Offset = 6;
    private const int ItemHeight = 6;
    private const int ItemsPerPage = 8;

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
            new("Astro Chicken", () => new AstroChicken.AstroChicken(fontFactory)),
            new("Bomber Man", () => new BombermanGame(fontFactory)),
            new("Breakout", () => new BreakoutGame()),
            new("Connect Four", () => new ConnectFourGame(), 100),
            new("Checkers", () => new CheckersGame(fontFactory)),
            new("Chess", () => new ChessGame()),
            new("E-Pong", () => new PongPlayableGame(fontFactory)),
            new("Flappy Bird", () => new FlappyBirdGame(fontFactory), 50),
            new("Frogger", () => new FroggerPlayableGame(), 100),
            new("Othello", () => new OthelloGame(fontFactory)),
            new("Master Mind 1", () => new Mastermind.MastermindGame(fontFactory)),
            new("Master Mind 2", () => new Mastermind.DuoMastermindGame(fontFactory)),
            new("Memory", () => new MemoryCardGame()),
            new("Mines", () => new MinesweeperGame(fontFactory)),
            new("Pong", () => new PongPlayableGame(fontFactory)),
            new("Pacific Wings", () => new PacificWingsGame(fontFactory)),
            new("Snake", () => new SnakePlayableGame(), 100),
            new("Snake 2", () => new SnakeGame2P(), 100),
            new("Space I", () => new SpaceInvadersPlayableGame(), 75),
            new("Tetris 1", () => new TetrisGame(fontFactory)),
            new("Tetris 2", () => new DuoTetrisGame(fontFactory)),
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
            //soundPlayer.Play(selectSound);
            runner.Run(game, displayInterval, name:name);
        }

        var r = random.Next(0, 4);
        console.LightButtons(r == 0, r == 1, r == 2, r == 3);
    }

    public void Update()
    {
    }

    public void Draw(IDisplay display)
    {
        display.DrawRectangle(0, 0, 64, 64, Color.Blue);
        var page = cursor / ItemsPerPage;
        for (var i=0; i < ItemsPerPage; i++)
        {
            var itemNo = page * ItemsPerPage + i;
            if (itemNo >= items.Length) break; 
            var item = items[itemNo];
            const int x = 1;
            var y = Offset + i * ItemHeight;
            if (cursor == itemNo)
            {
                display.DrawRectangle(x, y-5, 64, ItemHeight, Color.LightSkyBlue, Color.LightSkyBlue);
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