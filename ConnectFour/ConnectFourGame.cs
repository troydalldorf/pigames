using System.Drawing;
using Core;
using Core.Display.Sprites;
using Core.Inputs;

public class ConnectFourGame : IDuoGameElement
{
    private readonly Color[,] grid;
    private Color currentPlayer;
    private int selectedColumn;
    private const int Rows = 6;
    private const int Columns = 7;
    private const int CellSize = 8;
    private SpriteAnimation pieces;

    public ConnectFourGame()
    {
        grid = new Color[Rows, Columns];
        currentPlayer = Color.Red;
        selectedColumn = 0;
        State = GameOverState.None;
        var image = SpriteImage.FromResource("c4.png");
        pieces = image.GetSpriteAnimation(1, 1, 8, 8, 3, 1);
    }

    public void HandleInput(IPlayerConsole player1Console)
    {
        if (currentPlayer == Color.Blue) return;
        HandleInputInternal(player1Console);
    }

    public void Handle2PInput(IPlayerConsole player2Console)
    {
        if (currentPlayer == Color.Red) return;
        HandleInputInternal(new PlayerConsoleInversionDecorator(player2Console));
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
                    State = currentPlayer == Color.Red ? GameOverState.Player1Wins : GameOverState.Player2Wins;
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
        for (var y = 0; y < Rows; y++)
        {
            for (var x = 0; x < Columns; x++)
            {
                if (grid[y, x] == Color.Empty)
                    pieces.Draw(display, x * CellSize, y * CellSize, 2);
                else if (grid[y, x] == Color.Blue)
                    pieces.Draw(display, x * CellSize, y * CellSize, 0);
                else if (grid[y, x] == Color.Red)
                    pieces.Draw(display, x * CellSize, y * CellSize, 1);
            }
        }

        // Draw the current player's disk above the board
        var diskX = selectedColumn * CellSize + CellSize / 2;
        var diskY = (Rows + 1) * CellSize + CellSize / 2;
        display.DrawCircle(diskX, diskY, CellSize / 2 - 1, currentPlayer);
    }

    public GameOverState State { get; private set; }

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
