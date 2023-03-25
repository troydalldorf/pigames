using Core;
using Core.Display;
using Core.Display.Fonts;
using Core.Display.Sprites;
using Core.Inputs;

namespace Minesweeper;

using System;
using System.Drawing;
using System.Threading;

class MinesweeperGame
{
    private const int Width = 64;
    private const int Height = 64;
    private const int TileSize = 8;
    private const int NumMines = 10;

    private LedDisplay display;
    private LedFont font;
    private PlayerConsole playerConsole;
    private int[,] board;
    private bool[,] revealed;
    private bool[,] flagged;
    private int cursorX = 4*TileSize;
    private int cursorY = 4*TileSize;
    private bool gameOver = false;
    private readonly SpriteAnimation soilSprite;
    private readonly SpriteAnimation idSprite;
    private readonly SpriteAnimation cursorSprite;

    public MinesweeperGame(LedDisplay display, PlayerConsole playerConsole)
    {
        this.display = display;
        this.font = new LedFont(LedFontType.FontTomThumb);
        this.playerConsole = playerConsole;
        var image = SpriteImage.FromFile("ms.png", new Point(0, 46));
        soilSprite = image.GetSpriteAnimation(1, 1, 8, 8, 2, 1);
        idSprite = image.GetSpriteAnimation(1, 10, 8, 8, 2, 1);
        cursorSprite = image.GetSpriteAnimation(1, 19, 8, 8, 1, 1);
    }

    public void Run()
    {
        Initialize();

        while (true)
        {
            Update();
            Draw();
            Thread.Sleep(50);
        }
    }

    private void Initialize()
    {
        board = new int[Width / TileSize, Height / TileSize];
        revealed = new bool[Width / TileSize, Height / TileSize];
        flagged = new bool[Width / TileSize, Height / TileSize];

        PlaceMines();
        CalculateNumbers();
    }

    public void Update()
    {
        var stick = playerConsole.ReadJoystick();
        if (stick.IsUp())
        {
            cursorY -= TileSize;
            if (cursorY < 0)
                cursorY = 0;
        }
        else if (stick.IsDown())
        {
            cursorY += TileSize;
            if (cursorY >= Height)
                cursorY = Height - TileSize;
        }
        else if (stick.IsLeft())
        {
            cursorX -= TileSize;
            if (cursorX < 0)
                cursorX = 0;
        }
        else if (stick.IsRight())
        {
            cursorX += TileSize;
            if (cursorX >= Width)
                cursorX = Width - TileSize;
        }

        var tileX = cursorX / TileSize;
        var tileY = cursorY / TileSize;

        var buttons = playerConsole.ReadButtons();
        if (revealed[tileX, tileY]) return;
        if (buttons.IsRedPushed()) // Assuming button 1 is for revealing
        {
            if (board[tileX, tileY] == -1)
            {
                gameOver = true;
            }
            else
            {
                RevealEmpty(tileX, tileY);
            }
        }
        else if (buttons.IsGreenPushed()) // Assuming button 2 is for flagging
        {
            flagged[tileX, tileY] = !flagged[tileX, tileY];
        }
    }


    private void Draw()
    {
        display.Clear();

        for (var x = 0; x < Width / TileSize; x++)
        {
            for (var y = 0; y < Height / TileSize; y++)
            {
                var xPos = x * TileSize;
                var yPos = y * TileSize;

                if (revealed[x, y])
                {
                    soilSprite.Draw(display, xPos, yPos, 1);
                    if (board[x, y] > 0)
                    {
                        DrawNumber(xPos, yPos, board[x, y]);
                    }
                }
                else
                {
                    soilSprite.Draw(display, xPos, yPos, 0);
                    if (flagged[x, y])
                    {
                        idSprite.Draw(display, xPos, yPos, 0);
                    }
                }
            }
        }
        cursorSprite.Draw(display, cursorX, cursorY, 0);

        display.Update();
    }

    private void PlaceMines()
    {
        var minesPlaced = 0;
        var random = new Random();

        while (minesPlaced < NumMines)
        {
            var x = random.Next(0, Width / TileSize);
            var y = random.Next(0, Height / TileSize);

            if (board[x, y] == -1) continue;
            board[x, y] = -1;
            minesPlaced++;
        }
    }

    private void CalculateNumbers()
    {
        for (int x = 0; x < Width / TileSize; x++)
        {
            for (int y = 0; y < Height / TileSize; y++)
            {
                if (board[x, y] != -1)
                {
                    int adjacentMines = 0;

                    for (int xOffset = -1; xOffset <= 1; xOffset++)
                    {
                        for (int yOffset = -1; yOffset <= 1; yOffset++)
                        {
                            int newX = x + xOffset;
                            int newY = y + yOffset;

                            if (newX >= 0 && newX < Width / TileSize && newY >= 0 && newY < Height / TileSize && board[newX, newY] == -1)
                            {
                                adjacentMines++;
                            }
                        }
                    }

                    board[x, y] = adjacentMines;
                }
            }
        }
    }


    private void RevealEmpty(int x, int y)
    {
        if (x < 0 || x >= Width / TileSize || y < 0 || y >= Height / TileSize || revealed[x, y] || board[x, y] == -1) return;
        revealed[x, y] = true;

        if (board[x, y] != 0) return;
        for (var xOffset = -1; xOffset <= 1; xOffset++)
        {
            for (var yOffset = -1; yOffset <= 1; yOffset++)
            {
                RevealEmpty(x + xOffset, y + yOffset);
            }
        }
    }

    private void DrawNumber(int x, int y, int number)
    {
        font.DrawText(display, x+3, y-1, Color.DarkGreen, number.ToString());
    }
}