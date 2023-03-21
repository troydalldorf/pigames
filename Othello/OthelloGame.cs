using System;
using System.Drawing;
using Core;
using Core.Display.Fonts;

public class OthelloGame : I2PGameElement
{
    private const int GridSize = 8;

    private LedFont _font;

    private Grid _grid;
    private Player _currentPlayer;
    private bool _isDone;

    public OthelloGame()
    {
        _font = new LedFont(LedFontType.Font4x6);

        Initialize();
    }

    private void Initialize()
    {
        _grid = new Grid(GridSize, GridSize);
        _grid.PlacePiece(3, 3, Player.White);
        _grid.PlacePiece(4, 4, Player.White);
        _grid.PlacePiece(3, 4, Player.Black);
        _grid.PlacePiece(4, 3, Player.Black);

        _currentPlayer = Player.Black;
        _isDone = false;
    }
    
    public void HandleInput(IPlayerConsole player1Console)
    {
        if (_currentPlayer == Player.White) return;
        InternalHandleInput(player1Console);
    }
    
    public void Handle2PInput(IPlayerConsole player2Console)
    {
        if (_currentPlayer == Player.Black) return;
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

            _grid.MoveCursor(dx, dy);
        }

        if (buttons.HasFlag(Buttons.Red))
        {
            if (_grid.PlacePiece(_grid.CursorX, _grid.CursorY, _currentPlayer))
            {
                _currentPlayer = _currentPlayer == Player.Black ? Player.White : Player.Black;
            }
        }

        if (buttons.HasFlag(Buttons.Yellow))
        {
            _isDone = true;
        }
    }

    public void Update()
    {
    }

    public void Draw(IDisplay display)
    {
        _grid.Draw(display, _currentPlayer);

        if (_isDone)
        {
            int blackScore = _grid.Score(Player.Black);
            int whiteScore = _grid.Score(Player.White);

            if (blackScore > whiteScore)
            {
                _font.DrawText(display, 10, 30, Color.White, "Black Wins");
            }
            else if (whiteScore > blackScore)
            {
                _font.DrawText(display, 10, 30, Color.White, "White Wins");
            }
            else
            {
                _font.DrawText(display, 10, 30, Color.White, "Draw");
            }
        }
    }

    public bool IsDone() => _isDone;
}

public enum Player
{
    None,
    Black,
    White
}

public class Grid
{
    private int _width;
    private int _height;
    private Player[,] _board;
    private int _cursorX;
    private int _cursorY;
    
    public int CursorX
    {
        get => _cursorX;
        set => _cursorX = Math.Clamp(value, 0, _width - 1);
    }

    public int CursorY
    {
        get => _cursorY;
        set => _cursorY = Math.Clamp(value, 0, _height - 1);
    }

    public Grid(int width, int height)
    {
        _width = width;
        _height = height;
        _board = new Player[width, height];
        _cursorX = 0;
        _cursorY = 0;
    }

    public void MoveCursor(int dx, int dy)
    {
        CursorX += dx;
        CursorY += dy;
    }

    public bool PlacePiece(int x, int y, Player player)
    {
        if (_board[x, y] == Player.None && IsValidMove(x, y, player))
        {
            _board[x, y] = player;
            FlipPieces(x, y, player);
            return true;
        }

        return false;
    }

    public int Score(Player player)
    {
        int count = 0;
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                if (_board[x, y] == player)
                {
                    count++;
                }
            }
        }

        return count;
    }

    public void Draw(IDisplay display, Player currentPlayer)
    {
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                var borderColor = Color.Gray;
                var fillColor = Color.Pink;

                if (x == CursorX && y == CursorY)
                {
                    borderColor = currentPlayer == Player.Black ? Color.Blue : Color.Red;
                }

                if (_board[x, y] == Player.Black)
                {
                    fillColor = Color.Black;
                }
                else if (_board[x, y] == Player.White)
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

        while (x >= 0 && x < _width && y >= 0 && y < _height)
        {
            if (_board[x, y] == opponent)
            {
                count++;
            }
            else if (_board[x, y] == player && count > 0)
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
        Player opponent = player == Player.Black ? Player.White : Player.Black;

        x += dx;
        y += dy;

        while (x >= 0 && x < _width && y >= 0 && y < _height)
        {
            if (_board[x, y] == opponent)
            {
                _board[x, y] = player;
            }
            else if (_board[x, y] == player)
            {
                break;
            }

            x += dx;
            y += dy;
        }
    }
}