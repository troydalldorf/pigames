using System.Drawing;
using Core;
using Core.Display.Fonts;

public class CheckersGame : IGameElement
{
    private const int BoardSize = 8;
    private const int CellSize = 8;
    private const int Padding = 2;
    private const int PieceRadius = 3;

    private readonly LedFont font;

    private CheckerPiece[,] board;
    private CheckerPiece selectedPiece;
    private bool isPlayer1Turn;
    private readonly bool isDone;
    private int _cursorX;
    private int _cursorY;

    public CheckersGame()
    {
        font = new LedFont(LedFontType.Font4x6);

        InitializeBoard();
    }

    private void InitializeBoard()
    {
        board = new CheckerPiece[BoardSize, BoardSize];
        isPlayer1Turn = true;

        for (int y = 0; y < BoardSize; y++)
        {
            for (int x = 0; x < BoardSize; x++)
            {
                if ((x + y) % 2 == 1)
                {
                    if (y < 3)
                    {
                        board[x, y] = new CheckerPiece(true, false, x, y);
                    }
                    else if (y > 4)
                    {
                        board[x, y] = new CheckerPiece(false, false, x, y);
                    }
                }
            }
        }
    }

    public void HandleInput(IPlayerConsole player1Console)
    {
        var direction = player1Console.ReadJoystick();
        var buttons = player1Console.ReadButtons();

        if (!isPlayer1Turn)
        {
            return;
        }

        HandlePlayerInput(direction, buttons, true);
    }

    public void Handle2PInput(IPlayerConsole player2Console)
    {
        var direction = player2Console.ReadJoystick();
        var buttons = player2Console.ReadButtons();

        if (isPlayer1Turn)
        {
            return;
        }

        HandlePlayerInput(direction, buttons, false);
    }

    private void HandlePlayerInput(JoystickDirection direction, Buttons buttons, bool isPlayer1)
    {
        int dx = 0, dy = 0;
        switch (direction)
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

        var newX = _cursorX + dx;
        var newY = _cursorY + dy;

        if (newX >= 0 && newX < BoardSize && newY >= 0 && newY < BoardSize)
        {
            _cursorX = newX;
            _cursorY = newY;
        }

        if (buttons == Buttons.Green)
        {
            var piece = board[_cursorX, _cursorY];
            if (piece != null && piece.IsPlayer1 == isPlayer1)
            {
                selectedPiece = piece;
            }
            else if (selectedPiece != null)
            {
                var dxMove = Math.Abs(_cursorX - selectedPiece.X);
                var dyMove = _cursorY - selectedPiece.Y;
                var validMove = (dxMove == 1 && dyMove == (isPlayer1 ? -1 : 1)) ||
                                (dxMove == 2 && dyMove == (isPlayer1 ? -2 : 2));

                if (validMove)
                {
                    board[selectedPiece.X, selectedPiece.Y] = null;
                    selectedPiece.X = _cursorX;
                    selectedPiece.Y = _cursorY;
                    board[_cursorX, _cursorY] = selectedPiece;

                    if (_cursorY == 0 || _cursorY == BoardSize - 1)
                    {
                        selectedPiece.IsKing = true;
                    }

                    selectedPiece = null;
                    isPlayer1Turn = !isPlayer1Turn;
                }
            }
        }
        else if (buttons == Buttons.Red)
        {
            selectedPiece = null;
        }
    }


    public void Update()
    {
        // Add game logic/movement here
    }

    public void Draw(IDisplay display)
    {
        for (var y = 0; y < BoardSize; y++)
        {
            for (var x = 0; x < BoardSize; x++)
            {
                var xPos = x * CellSize + Padding;
                var yPos = y * CellSize + Padding;

                var cellColor = (x + y) % 2 == 0 ? Color.Black : Color.DarkGray;
                display.DrawRectangle(xPos, yPos, CellSize, CellSize, cellColor, cellColor);

                var piece = board[x, y];
                if (piece != null)
                {
                    var pieceColor = piece.IsPlayer1 ? Color.Red : Color.Blue;
                    display.DrawCircle(xPos + CellSize / 2, yPos + CellSize / 2, PieceRadius, pieceColor);
                }
            }
        }

        // cursor
        display.DrawRectangle(_cursorX * CellSize, _cursorY * CellSize, CellSize, CellSize, Color.Lime);
        // Draw the player's turn
        var playerTurnText = isPlayer1Turn ? "Player 1" : "Player 2";
        var turnTextColor = isPlayer1Turn ? Color.Red : Color.Blue;
        font.DrawText(display, 1, 32, turnTextColor, playerTurnText);
    }

    public bool IsDone() => isDone;

    private class CheckerPiece
    {
        public bool IsPlayer1 { get; }
        public bool IsKing { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        public CheckerPiece(bool isPlayer1, bool isKing, int x, int y)
        {
            IsPlayer1 = isPlayer1;
            IsKing = isKing;
            X = x;
            Y = y;
        }
    }
}