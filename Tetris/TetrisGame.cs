using System.Diagnostics;
using System.Drawing;
using Core;
using Core.Effects;

namespace Tetris;

class TetrisGame : IGameElement
{
    private const int Width = 10;
    private const int Height = 20;
    private const int PixelSize = 3;

    private int[,] grid;
    private Tetromino currentTetromino;
    private int currentX, currentY;
    private int speed;
    private int frame;
    private List<PixelBomb> pixelBombs = new();

    private Random random;
    private Stopwatch stopwatch;
    private long lastActionAt;

    public TetrisGame()
    {
        this.stopwatch = new Stopwatch();
        this.stopwatch.Start();

        grid = new int[Width, Height];
        random = new Random();
        speed = 10;
        frame = 0;

        NewTetromino();
    }

    public void HandleInput(IPlayerConsole playerConsole)
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

    public void Update()
    {
        frame++;

        foreach (var bomb in pixelBombs.ToArray())
        {
            bomb.Update();
            if (bomb.IsExtinguished()) pixelBombs.Remove(bomb);
        }
        
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

    public void Draw(IDisplay display)
    {
        display.Clear();
        display.DrawRectangle(0, 0, Width*PixelSize+2, Height*PixelSize+2, Color.DarkGray);
        
        foreach (var bomb in pixelBombs)
        {
            bomb.Draw(display);
        }

        // Draw grid
        for (var x = 0; x < Width; x++)
        {
            for (var y = 0; y < Height; y++)
            {
                if (grid[x, y] > 0)
                {
                    display.DrawRectangle(1+ x * PixelSize, 1 + y * PixelSize, PixelSize, PixelSize, Tetromino.GetColor(grid[x, y]));
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
                    display.DrawRectangle(1+ (currentX + x) * PixelSize, 1+ (currentY + y) * PixelSize, PixelSize, PixelSize, Tetromino.GetColor(currentTetromino.Type));
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

                if (boardX is < 0 or >= Width || boardY is < 0 or >= Height || grid[boardX, boardY] > 0)
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
                for (var x = 0; x < Width; x++)
                {
                    pixelBombs.Add(new PixelBomb(1+ x*PixelSize, y*PixelSize, PixelSize*PixelSize, Tetromino.GetColor(grid[x,y])));
                }
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