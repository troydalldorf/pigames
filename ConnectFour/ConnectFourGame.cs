using System.Drawing;
using Core;

public class ConnectFourGame : IGameElement
{
    private IDisplay _display;
    private IPlayerConsole _player1Console;
    private IPlayerConsole _player2Console;
    private Color[,] _grid;
    private Color _currentPlayer;
    private int _selectedColumn;
    private bool _isDone;
    private const int Rows = 6;
    private const int Columns = 7;
    private const int CellSize = 8;

    public ConnectFourGame(IDisplay display, IPlayerConsole player1Console, IPlayerConsole player2Console)
    {
        _display = display;
        _player1Console = player1Console;
        _player2Console = player2Console;
        _grid = new Color[Rows, Columns];
        _currentPlayer = Color.Red;
        _selectedColumn = 0;
        _isDone = false;
    }

    public void HandleInput(IPlayerConsole playerConsole)
    {
        var joystick = playerConsole.ReadJoystick();
        if (joystick.HasFlag(JoystickDirection.Left) && _selectedColumn > 0)
            _selectedColumn--;
        if (joystick.HasFlag(JoystickDirection.Right) && _selectedColumn < Columns - 1)
            _selectedColumn++;

        if (playerConsole.ReadButtons().HasFlag(Buttons.Green))
        {
            if (TryDropDisk(_selectedColumn))
            {
                if (CheckForWin(_currentPlayer))
                {
                    _isDone = true;
                    return;
                }

                _currentPlayer = _currentPlayer == Color.Red ? Color.Blue : Color.Red;
            }
        }
    }

    public void Update()
    {
        HandleInput(_currentPlayer == Color.Red ? _player1Console : _player2Console);
    }

    public void Draw(IDisplay display)
    {
        // Draw the static game board
        Color darkYellow = Color.FromArgb(128, 128, 0);
        for (int y = 0; y < Rows; y++)
        {
            for (int x = 0; x < Columns; x++)
            {
                display.DrawRectangle(x * CellSize, y * CellSize, CellSize, CellSize, darkYellow, darkYellow);
            }
        }

        // Draw the holes
        for (int y = 0; y < Rows; y++)
        {
            for (int x = 0; x < Columns; x++)
            {
                if (_grid[y, x] == Color.Empty)
                {
                    display.DrawCircle(x * CellSize + CellSize / 2, y * CellSize + CellSize / 2, CellSize / 2 - 1, Color.Black);
                }
            }
        }

        // Draw the dropped disks
        for (int y = 0; y < Rows; y++)
        {
            for (int x = 0; x < Columns; x++)
            {
                if (_grid[y, x] != Color.Empty)
                {
                    display.DrawCircle(x * CellSize + CellSize / 2, y * CellSize + CellSize / 2, CellSize / 2 - 1, _grid[y, x]);
                }
            }
        }

        // Draw the current player's disk above the board
        int diskX = _selectedColumn * CellSize + CellSize / 2;
        int diskY = (Rows + 1) * CellSize + CellSize / 2;
        display.DrawCircle(diskX, diskY, CellSize / 2 - 1, _currentPlayer);
    }

    public GameOverState State => _isDone ? GameOverState.EndOfGame : GameOverState.None;

    private bool TryDropDisk(int column)
    {
        for (int row = Rows - 1; row >= 0; row--)
        {
            if (_grid[row, column] == Color.Empty)
            {
                _grid[row, column] = _currentPlayer;
                return true;
            }
        }

        return false;
    }

    private bool CheckForWin(Color player)
    {
        for (int y = 0; y < Rows; y++)
        {
            for (int x = 0; x < Columns; x++)
            {
                if (_grid[y, x] == player &&
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
            if (_grid[y, x] == player)
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
