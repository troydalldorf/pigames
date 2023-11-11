using System.Diagnostics;
using Core;
using Core.Display;
using Core.Fonts;
using Core.Inputs;
using Core.State;

namespace Minesweeper;

using System;
using System.Drawing;

public class MinesweeperGame : IPlayableGameElement
{
    // Allow clicking a "1" when there is one adjacent mine, and clear all others, or blow if it's incorrectly flagged.
    private const int Width = 64;
    private const int Height = 64;
    private const int TileSize = 8;
    private const int NumMines = 10;

    private IFont font;
    private int[,] board;
    private bool[,] revealed;
    private bool[,] flagged;
    private int cursorX = 4 * TileSize;
    private int cursorY = 4 * TileSize;
    private bool gameOver = false;
    private readonly Stopwatch stopwatch = new Stopwatch();
    private long lastActionAt;

    public MinesweeperGame(IFontFactory fontFactory)
    {
        font = fontFactory.GetFont(LedFontType.FontTomThumb);
        Initialize();
    }

    private void Initialize()
    {
        stopwatch.Start();
        board = new int[Width / TileSize, Height / TileSize];
        revealed = new bool[Width / TileSize, Height / TileSize];
        flagged = new bool[Width / TileSize, Height / TileSize];
        State = GameOverState.None;
        PlaceMines();
        CalculateNumbers();
    }

    public void HandleInput(IPlayerConsole player1Console)
    {
        if (stopwatch.ElapsedMilliseconds - lastActionAt < 120)
            return;
        var stick = player1Console.ReadJoystick();
        if (stick.IsUp())
        {
            cursorY -= TileSize;
            if (cursorY < 0)
                cursorY = 0;
            lastActionAt = stopwatch.ElapsedMilliseconds;
        }
        else if (stick.IsDown())
        {
            cursorY += TileSize;
            if (cursorY > Height - TileSize)
                cursorY = Height - TileSize;
            lastActionAt = stopwatch.ElapsedMilliseconds;
        }
        else if (stick.IsLeft())
        {
            cursorX -= TileSize;
            if (cursorX < 0)
                cursorX = 0;
            lastActionAt = stopwatch.ElapsedMilliseconds;
        }
        else if (stick.IsRight())
        {
            cursorX += TileSize;
            if (cursorX > Width - TileSize)
                cursorX = Width - TileSize;
            lastActionAt = stopwatch.ElapsedMilliseconds;
        }

        var tileX = cursorX / TileSize;
        var tileY = cursorY / TileSize;

        var buttons = player1Console.ReadButtons();
        if (revealed[tileX, tileY]) return;
        if (buttons.IsGreenPushed()) // Assuming button 1 is for revealing
        {
            if (board[tileX, tileY] == -1)
            {
                State = GameOverState.EndOfGame;
            }
            else
            {
                RevealEmpty(tileX, tileY);
                if (CheckWinCondition())
                {
                    State = GameOverState.Player1Wins;
                }
            }
            lastActionAt = stopwatch.ElapsedMilliseconds;
        }
        else if (buttons.IsBluePushed()) // Assuming button 2 is for flagging
        {
            flagged[tileX, tileY] = !flagged[tileX, tileY];
            if (CheckWinCondition())
            {
                State = GameOverState.Player1Wins;
            }
            lastActionAt = stopwatch.ElapsedMilliseconds;
        }
    }
    
    public bool CheckWinCondition()
    {
        for (var x = 0; x < Width / TileSize; x++)
        {
            for (var y = 0; y < Height / TileSize; y++)
            {
                if (board[x, y] != -1 && !revealed[x, y])
                {
                    return false;
                }
            }
        }

        return true;
    }


    public void Update()
    {
    }

    public void Draw(IDisplay display)
    {
        for (var x = 0; x < Width / TileSize; x++)
        {
            for (var y = 0; y < Height / TileSize; y++)
            {
                var xPos = x * TileSize;
                var yPos = y * TileSize;

                if (revealed[x, y])
                {
                    display.DrawRectangle(xPos, yPos, TileSize, TileSize, Color.LightBlue, Color.LightBlue);
                    if (board[x, y] > 0)
                    {
                        DrawNumber(display, xPos, yPos, board[x, y]);
                    }
                }
                else
                {
                    display.DrawRectangle(xPos, yPos, TileSize, TileSize, Color.LightBlue);
                    if (flagged[x, y])
                    {
                        DrawFlag(display, xPos, yPos, Color.Red);
                    }
                }
            }
        }
        display.DrawRectangle(cursorX, cursorY, TileSize, TileSize, Color.GreenYellow);
    }

    public GameOverState State { get; private set; }

    private static void DrawFlag(IDisplay display, int xPos, int yPos, Color color)
    {
        int flagPoleX = xPos + TileSize / 2;
        int flagPoleY1 = yPos + TileSize / 4;
        int flagPoleY2 = yPos + 3 * TileSize / 4;

        int flagTopX = xPos + TileSize / 2;
        int flagTopY = yPos + TileSize / 4;
        int flagBottomX = xPos + 3 * TileSize / 4;
        int flagBottomY = yPos + TileSize / 2;

        // Draw the flag pole
        display.DrawLine(flagPoleX, flagPoleY1, flagPoleX, flagPoleY2, color);

        // Draw the flag triangle
        display.DrawLine(flagTopX, flagTopY, flagBottomX, flagTopY, color);
        display.DrawLine(flagTopX, flagTopY, flagBottomX, flagBottomY, color);
        display.DrawLine(flagBottomX, flagTopY, flagBottomX, flagBottomY, color);
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
        if (x < 0 || x >= Width / TileSize || y < 0 || y >= Height / TileSize || revealed[x, y] || flagged[x, y] || board[x, y] == -1) return;
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

    private void DrawNumber(IDisplay display, int x, int y, int number)
    {
        font.DrawText(display, x + 3, y + 6, Color.DarkBlue, number.ToString());
    }
}