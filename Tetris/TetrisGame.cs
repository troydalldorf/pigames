using System.Diagnostics;
using Core.Inputs;

namespace Tetris;

using System;
using System.Threading;
using Core.Display;

class TetrisGame
{
    private const int Width = 10;
    private const int Height = 20;
    private const int PixelSize = 3;

    private LedDisplay display;
    private PlayerConsole playerConsole;

    private int[,] grid;
    private Tetromino currentTetromino;
    private int currentX, currentY;
    private int speed;
    private int frame;

    private Random random;
    private Stopwatch stopwatch;
    private long lastActionAt;

    public TetrisGame(LedDisplay display, PlayerConsole playerConsole)
    {
        this.display = display;
        this.playerConsole = playerConsole;
        this.stopwatch = new Stopwatch();
        this.stopwatch.Start();

        grid = new int[Width, Height];
        random = new Random();
        speed = 10;
        frame = 0;

        NewTetromino();
    }

    public void Run()
    {
        while (true)
        {
            HandleInput();
            Update();
            Draw();
            Thread.Sleep(50);
        }
    }

    private void HandleInput()
    {
        if (stopwatch.ElapsedMilliseconds - lastActionAt < 120)
            return;
        var stick = playerConsole.ReadJoystick();

        if (stick.IsLeft())
        {
            if (IsValidMove(currentX - 1, currentY, currentTetromino))
            {
                currentX--;
                lastActionAt = stopwatch.ElapsedMilliseconds;
            }
        }
        if (stick.IsRight())
        {
            if (IsValidMove(currentX + 1, currentY, currentTetromino))
            {
                currentX++;
                lastActionAt = stopwatch.ElapsedMilliseconds;
            }
        }
        if (stick.IsDown())
        {
            if (IsValidMove(currentX, currentY + 1, currentTetromino))
            {
                currentY++;
            }
        }
        if (stick.IsUp())
        {
            var rotated = currentTetromino.RotateRight();
            if (!IsValidMove(currentX, currentY, rotated)) return;
            currentTetromino = rotated;
            lastActionAt = stopwatch.ElapsedMilliseconds;
        }
    }

    private void Update()
    {
        frame++;

        if (frame % speed != 0) return;
        if (IsValidMove(currentX, currentY + 1, currentTetromino))
        {
            currentY++;
        }
        else
        {
            MergeTetromino();
            ClearLines();
            NewTetromino();

            if (!IsValidMove(currentX, currentY, currentTetromino))
            {
                // Game over
                grid = new int[Width, Height];
            }
        }
    }

    private void Draw()
    {
        display.Clear();

        // Draw grid
        for (var x = 0; x < Width; x++)
        {
            for (var y = 0; y < Height; y++)
            {
                if (grid[x, y] > 0)
                {
                    display.DrawRectangle(x * PixelSize, y * PixelSize, PixelSize, PixelSize, Tetromino.GetColor(grid[x, y]));
                }
            }
        }

        // Draw current tetromino
        for (var x = 0; x < currentTetromino.Width; x++)
        {
            for (var y = 0; y < currentTetromino.Height; y++)
            {
                if (currentTetromino[x, y])
                {
                    display.DrawRectangle((currentX + x) * PixelSize, (currentY + y) * PixelSize, PixelSize, PixelSize, Tetromino.GetColor(currentTetromino.Type));
                }
            }
        }

        display.Update();
    }

    private bool IsValidMove(int newX, int newY, Tetromino tetromino)
    {
        for (var x = 0; x < tetromino.Width; x++)
        {
            for (var y = 0; y < tetromino.Height; y++)
            {
                if (!tetromino[x, y]) continue;
                var boardX = newX + x;
                var boardY = newY + y;

                if (boardX < 0 || boardX >= Width || boardY < 0 || boardY >= Height || grid[boardX, boardY] > 0)
                {
                    return false;
                }
            }
        }
        return true;
    }

    private void MergeTetromino()
    {
        for (var x = 0; x < currentTetromino.Width; x++)
        {
            for (var y = 0; y < currentTetromino.Height; y++)
            {
                if (currentTetromino[x, y])
                {
                    grid[currentX + x, currentY + y] = currentTetromino.Type;
                }
            }
        }
    }

    private void ClearLines()
    {
        for (var y = Height - 1; y >= 0; y--)
        {
            var fullLine = true;

            for (var x = 0; x < Width; x++)
            {
                if (grid[x, y] != 0) continue;
                fullLine = false;
                break;
            }

            if (fullLine)
            {
                for (var yy = y; yy > 0; yy--)
                {
                    for (var x = 0; x < Width; x++)
                    {
                        grid[x, yy] = grid[x, yy - 1];
                    }
                }
                y++;
            }
        }
    }

    private void NewTetromino()
    {
        currentTetromino = Tetromino.RandomTetromino(random);
        currentX = Width / 2 - currentTetromino.Width / 2;
        currentY = 0;
    }
}