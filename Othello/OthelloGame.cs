using System.Drawing;
using Core;
using Core.Display.Fonts;

namespace Othello;

public class OthelloGame : I2PGameElement
{
    private const int GridSize = 8;

    private readonly LedFont font;

    private Grid grid;
    private Player currentPlayer;
    private bool isDone;

    public OthelloGame()
    {
        font = new LedFont(LedFontType.Font4x6);
        Initialize();
    }

    private void Initialize()
    {
        grid = new Grid(GridSize, GridSize);
        grid.PlacePiece(3, 3, Player.White, true);
        grid.PlacePiece(4, 4, Player.White, true);
        grid.PlacePiece(3, 4, Player.Black, true);
        grid.PlacePiece(4, 3, Player.Black, true);

        currentPlayer = Player.Black;
        isDone = false;
    }
    
    public void HandleInput(IPlayerConsole player1Console)
    {
        if (currentPlayer == Player.Black) return;
        InternalHandleInput(player1Console);
    }
    
    public void Handle2PInput(IPlayerConsole player2Console)
    {
        if (currentPlayer == Player.White) return;
        InternalHandleInput(player2Console);
    }
    
    private void InternalHandleInput(IPlayerConsole console)
    {
        var joystickDirection = console.ReadJoystick();
        var buttons = console.ReadButtons();

        if (joystickDirection != JoystickDirection.None)
        {
            int dx = 0, dy = 0;

            switch (joystickDirection)
            {
                case JoystickDirection.Up:
                    dy = -1;
                    break;
                case JoystickDirection.Down:
                    dy = 1;
                    break;
                case JoystickDirection.Left:
                    dx = -1;
                    break;
                case JoystickDirection.Right:
                    dx = 1;
                    break;
            }

            grid.MoveCursor(dx, dy);
        }

        if (buttons.HasFlag(Buttons.Green))
        {
            if (grid.PlacePiece(grid.CursorX, grid.CursorY, currentPlayer, false))
            {
                currentPlayer = currentPlayer == Player.Black ? Player.White : Player.Black;
            }
        }

        if (buttons.HasFlag(Buttons.Yellow))
        {
            isDone = true;
        }
    }

    public void Update()
    {
    }

    public void Draw(IDisplay display)
    {
        grid.Draw(display, currentPlayer);

        if (isDone)
        {
            var blackScore = grid.Score(Player.Black);
            var whiteScore = grid.Score(Player.White);

            if (blackScore > whiteScore)
            {
                font.DrawText(display, 10, 30, Color.White, "Black Wins");
            }
            else if (whiteScore > blackScore)
            {
                font.DrawText(display, 10, 30, Color.White, "White Wins");
            }
            else
            {
                font.DrawText(display, 10, 30, Color.White, "Draw");
            }
        }
    }

    public bool IsDone() => isDone;
}

public enum Player
{
    None,
    Black,
    White
}

public class Grid
{
    private readonly int width;
    private readonly int height;
    private readonly Player[,] board;
    private int cursorX;
    private int cursorY;
    
    public int CursorX
    {
        get => cursorX;
        set => cursorX = Math.Clamp(value, 0, width - 1);
    }

    public int CursorY
    {
        get => cursorY;
        set => cursorY = Math.Clamp(value, 0, height - 1);
    }

    public Grid(int width, int height)
    {
        this.width = width;
        this.height = height;
        board = new Player[width, height];
        cursorX = 0;
        cursorY = 0;
    }

    public void MoveCursor(int dx, int dy)
    {
        CursorX += dx;
        CursorY += dy;
    }

    public bool PlacePiece(int x, int y, Player player, bool skipCheck)
    {
        if (board[x, y] == Player.None && (skipCheck || IsValidMove(x, y, player)))
        {
            board[x, y] = player;
            FlipPieces(x, y, player);
            return true;
        }

        return false;
    }

    public int Score(Player player)
    {
        int count = 0;
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (board[x, y] == player)
                {
                    count++;
                }
            }
        }

        return count;
    }

    public void Draw(IDisplay display, Player currentPlayer)
    {
        for (var x = 0; x < width; x++)
        {
            for (var y = 0; y < height; y++)
            {
                var borderColor = Color.DarkSlateGray;
                Color? fillColor = null;

                if (x == CursorX && y == CursorY)
                {
                    borderColor = currentPlayer == Player.Black ? Color.Blue : Color.Red;
                }

                if (board[x, y] == Player.Black)
                {
                    fillColor = Color.Black;
                }
                else if (board[x, y] == Player.White)
                {
                    fillColor = Color.White;
                }

                display.DrawRectangle(x * 8, y * 8, 8, 8, borderColor, fillColor);
            }
        }
    }

    private bool IsValidMove(int x, int y, Player player)
    {
        for (int dx = -1; dx <= 1; dx++)
        {
            for (int dy = -1; dy <= 1; dy++)
            {
                if (dx == 0 && dy == 0)
                {
                    continue;
                }

                if (CheckDirection(x, y, dx, dy, player))
                {
                    return true;
                }
            }
        }

        return false;
    }

    private bool CheckDirection(int x, int y, int dx, int dy, Player player)
    {
        int count = 0;
        Player opponent = player == Player.Black ? Player.White : Player.Black;

        x += dx;
        y += dy;

        while (x >= 0 && x < width && y >= 0 && y < height)
        {
            if (board[x, y] == opponent)
            {
                count++;
            }
            else if (board[x, y] == player && count > 0)
            {
                return true;
            }
            else
            {
                break;
            }

            x += dx;
            y += dy;
        }

        return false;
    }

    private void FlipPieces(int x, int y, Player player)
    {
        for (int dx = -1; dx <= 1; dx++)
        {
            for (int dy = -1; dy <= 1; dy++)
            {
                if (dx == 0 && dy == 0)
                {
                    continue;
                }

                if (CheckDirection(x, y, dx, dy, player))
                {
                    FlipDirection(x, y, dx, dy, player);
                }
            }
        }
    }

    private void FlipDirection(int x, int y, int dx, int dy, Player player)
    {
        var opponent = player == Player.Black ? Player.White : Player.Black;

        x += dx;
        y += dy;

        while (x >= 0 && x < width && y >= 0 && y < height)
        {
            if (board[x, y] == opponent)
            {
                board[x, y] = player;
            }
            else if (board[x, y] == player)
            {
                break;
            }

            x += dx;
            y += dy;
        }
    }
}