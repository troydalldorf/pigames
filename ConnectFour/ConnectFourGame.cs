using System.Drawing;
using Core;
using Core.Display.Sprites;
using Core.Inputs;

namespace ConnectFour;

public class ConnectFourGame : IDuoPlayableGameElement
{
    private readonly Player[,] grid;
    private Player currentPlayer;
    private int selectedColumn;
    private const int Rows = 6;
    private const int Columns = 7;
    private const int CellSize = 8;
    private readonly SpriteAnimation gridPieces;
    private readonly SpriteAnimation dropPieces;

    public ConnectFourGame()
    {
        grid = new Player[Rows, Columns];
        currentPlayer = Player.Red;
        selectedColumn = 0;
        State = GameOverState.None;
        var image = SpriteImage.FromResource("c4.png");
        gridPieces = image.GetSpriteAnimation(1, 1, 8, 8, 3, 1);
        image = SpriteImage.FromResource("c4.png", new Point(1, 1));
        dropPieces = image.GetSpriteAnimation(1, 1, 8, 8, 3, 1);
    }

    public void HandleInput(IPlayerConsole player1Console)
    {
        if (currentPlayer == Player.Blue) return;
        HandleInputInternal(player1Console);
    }

    public void Handle2PInput(IPlayerConsole player2Console)
    {
        if (currentPlayer == Player.Red) return;
        HandleInputInternal(new PlayerConsoleInversionDecorator(player2Console));
    }

    private void HandleInputInternal(IPlayerConsole playerConsole)
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
                    State = currentPlayer == Player.Red ? GameOverState.Player1Wins : GameOverState.Player2Wins;
                    return;
                }

                currentPlayer = currentPlayer ==Player.Red ? Player.Blue : Player.Red;
            }
        }
    }

    public void Update()
    {
    }

    public void Draw(IDisplay display)
    {
        const int xOffset = 4;
        const int yOffset = 12;
        for (var y = 0; y < Rows; y++)
        {
            for (var x = 0; x < Columns; x++)
                gridPieces.Draw(display, xOffset + x * CellSize, yOffset + y * CellSize, (int)grid[y, x]);
        }

        // Draw the current player's disk above the board
        var diskX = xOffset + selectedColumn * CellSize;
        const int diskY = yOffset - CellSize-1;
        dropPieces.Draw(display, diskX, diskY, (int)currentPlayer);
    }

    public GameOverState State { get; private set; }

    private bool TryDropDisk(int column)
    {
        for (var row = Rows - 1; row >= 0; row--)
        {
            if (grid[row, column] != Player.Empty) continue;
            grid[row, column] = currentPlayer;
            return true;
        }

        return false;
    }

    private bool CheckForWin(Player player)
    {
        for (var y = 0; y < Rows; y++)
        {
            for (var x = 0; x < Columns; x++)
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

    private bool CheckForWinDirection(int x, int y, int dx, int dy, Player player)
    {
        var count = 0;

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

    private enum Player
    {
        Empty = 0,
        Blue  = 1,
        Red   = 2
    }
}