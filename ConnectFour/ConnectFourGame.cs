using System.Drawing;
using Core;

public class ConnectFourGame : IDuoGameElement
{
    private readonly Color[,] grid;
    private Color currentPlayer;
    private int selectedColumn;
    private bool isDone;
    private const int Rows = 6;
    private const int Columns = 7;
    private const int CellSize = 8;

    public ConnectFourGame()
    {
        grid = new Color[Rows, Columns];
        currentPlayer = Color.Red;
        selectedColumn = 0;
        isDone = false;
    }

    public void HandleInput(IPlayerConsole player1Console)
    {
        HandleInputInternal(player1Console);
    }

    public void Handle2PInput(IPlayerConsole player2Console)
    {
        HandleInputInternal(player2Console);
    }
    
    public void HandleInputInternal(IPlayerConsole playerConsole)
    {
        var joystick = playerConsole.ReadJoystick();
        if (joystick.HasFlag(JoystickDirection.Left) && selectedColumn > 0)
            selectedColumn--;
        if (joystick.HasFlag(JoystickDirection.Right) && selectedColumn < Columns - 1)
            selectedColumn++;

        if (playerConsole.ReadButtons().HasFlag(Buttons.Green))
        {
            if (TryDropDisk(selectedColumn))
            {
                if (CheckForWin(currentPlayer))
                {
                    isDone = true;
                    return;
                }

                currentPlayer = currentPlayer == Color.Red ? Color.Blue : Color.Red;
            }
        }
    }

    public void Update()
    {
    }

    public void Draw(IDisplay display)
    {
        // Draw the static game board
        var darkYellow = Color.FromArgb(128, 128, 0);
        for (var y = 0; y < Rows; y++)
        {
            for (var x = 0; x < Columns; x++)
            {
                display.DrawRectangle(x * CellSize, y * CellSize, CellSize, CellSize, darkYellow, darkYellow);
            }
        }

        // Draw the holes
        for (var y = 0; y < Rows; y++)
        {
            for (var x = 0; x < Columns; x++)
            {
                if (grid[y, x] == Color.Empty)
                {
                    display.DrawCircle(x * CellSize + CellSize / 2, y * CellSize + CellSize / 2, CellSize / 2 - 1, Color.Black);
                }
            }
        }

        // Draw the dropped disks
        for (var y = 0; y < Rows; y++)
        {
            for (var x = 0; x < Columns; x++)
            {
                if (grid[y, x] != Color.Empty)
                {
                    display.DrawCircle(x * CellSize + CellSize / 2, y * CellSize + CellSize / 2, CellSize / 2 - 1, grid[y, x]);
                }
            }
        }

        // Draw the current player's disk above the board
        int diskX = selectedColumn * CellSize + CellSize / 2;
        int diskY = (Rows + 1) * CellSize + CellSize / 2;
        display.DrawCircle(diskX, diskY, CellSize / 2 - 1, currentPlayer);
    }

    public GameOverState State => isDone ? GameOverState.EndOfGame : GameOverState.None;

    private bool TryDropDisk(int column)
    {
        for (var row = Rows - 1; row >= 0; row--)
        {
            if (grid[row, column] != Color.Empty) continue;
            grid[row, column] = currentPlayer;
            return true;
        }

        return false;
    }

    private bool CheckForWin(Color player)
    {
        for (int y = 0; y < Rows; y++)
        {
            for (int x = 0; x < Columns; x++)
            {
                if (grid[y, x] == player &&
                    (CheckForWinDirection(x, y, 1, 0, player) ||
                     CheckForWinDirection(x, y, 0, 1, player) ||
                     CheckForWinDirection(x, y, 1, 1, player) ||
                     CheckForWinDirection(x, y, 1, -1, player)))
                {
                    return true;
                }
            }
        }

        return false;
    }

    private bool CheckForWinDirection(int x, int y, int dx, int dy, Color player)
    {
        int count = 0;

        while (x >= 0 && x < Columns && y >= 0 && y < Rows)
        {
            if (grid[y, x] == player)
            {
                count++;
                if (count == 4)
                    return true;
            }
            else
            {
                count = 0;
            }

            x += dx;
            y += dy;
        }

        return false;
    }
}
