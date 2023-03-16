using Core.Display;
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
    private PlayerConsole playerConsole;
    private int[,] board;
    private bool[,] revealed;
    private bool[,] flagged;

    public MinesweeperGame(LedDisplay display, PlayerConsole playerConsole)
    {
        this.display = display;
        this.playerConsole = playerConsole;
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

    private void Update()
    {
        var stick = playerConsole.ReadJoystick();
        int x = stick.X / TileSize;
        int y = stick.Y / TileSize;

        var buttons = playerConsole.ReadButtons();
        if (buttons > 0)
        {
            if (!flagged[x, y])
            {
                revealed[x, y] = true;
                if (board[x, y] == -1)
                {
                    Initialize();
                }
                else if (board[x, y] == 0)
                {
                    RevealEmpty(x, y);
                }
            }
        }
        else if (buttons < 0)
        {
            flagged[x, y] = !flagged[x, y];
        }
    }

    private void Draw()
    {
        display.Clear();

        for (int x = 0; x < Width / TileSize; x++)
        {
            for (int y = 0; y < Height / TileSize; y++)
            {
                int xPos = x * TileSize;
                int yPos = y * TileSize;

                if (revealed[x, y])
                {
                    display.DrawRectangle(xPos, yPos, TileSize, TileSize, Color.LightGray);

                    if (board[x, y] > 0)
                    {
                        // You can use a custom DrawNumber method to draw the numbers on the board
                        DrawNumber(xPos, yPos, board[x, y]);
                    }
                }
                else
                {
                    display.DrawRectangle(xPos, yPos, TileSize, TileSize, Color.Gray);

                    if (flagged[x, y])
                    {
                        // You can use a custom DrawFlag method to draw a flag symbol on the board
                        DrawFlag(xPos, yPos);
                    }
                }
            }
        }

        display.Update();
    }

    private void PlaceMines()
    {
        int minesPlaced = 0;
        Random random = new Random();

        while (minesPlaced < NumMines)
        {
            int x = random.Next(0, Width / TileSize);
            int y = random.Next(0, Height / TileSize);

            if (board[x, y] != -1)
            {
                board[x, y] = -1;
                minesPlaced++;
            }
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
        if (x >= 0 && x < Width / TileSize && y >= 0 && y < Height / TileSize && !revealed[x, y] && board[x, y] != -1)
        {
            revealed[x, y] = true;

            if (board[x, y] == 0)
            {
                for (int xOffset = -1; xOffset <= 1; xOffset++)
                {
                    for (int yOffset = -1; yOffset <= 1; yOffset++)
                    {
                        RevealEmpty(x + xOffset, y + yOffset);
                    }
                }
            }
        }
    }

    private void DrawNumber(int x, int y, int number)
    {
        Color color = Color.Black;

        switch (number)
        {
            case 1:
                color = Color.Blue;
                break;
            case 2:
                color = Color.Green;
                break;
            case 3:
                color = Color.Red;
                break;
            case 4:
                color = Color.DarkBlue;
                break;
            case 5:
                color = Color.DarkRed;
                break;
            case 6:
                color = Color.DarkCyan;
                break;
            case 7:
                color = Color.Black;
                break;
            case 8:
                color = Color.Gray;
                break;
        }

        int centerX = x + (TileSize / 2);
        int centerY = y + (TileSize / 2);
        display.SetPixel(centerX, centerY, color);
    }

    private void DrawFlag(int x, int y)
    {
        int centerX = x + (TileSize / 2);
        int centerY = y + (TileSize / 2);

        Color color = Color.Red;
        int flagWidth = TileSize / 4;
        int flagHeight = TileSize / 4;

        // Draw flagpole
        display.DrawLine(centerX - (flagWidth / 2), centerY - (flagHeight / 2), centerX - (flagWidth / 2), centerY + (flagHeight / 2), Color.Black);

        // Draw flag
        display.DrawLine(centerX - (flagWidth / 2), centerY - (flagHeight / 2), centerX + (flagWidth / 2), centerY - (flagHeight / 2), color);
        display.DrawLine(centerX + (flagWidth / 2), centerY - (flagHeight / 2), centerX - (flagWidth / 2), centerY, color);
        display.DrawLine(centerX - (flagWidth / 2), centerY, centerX - (flagWidth / 2), centerY - (flagHeight / 2), color);
    }
}