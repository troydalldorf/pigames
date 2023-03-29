using System.Drawing;
using Checkers.Bits;
using Core;
using Core.Display.Fonts;

public class CheckersGame : IDuoPlayableGameElement
{
    private const int BoardSize = 8;
    private const int CellSize = 8;
    private const int Padding = 0;
    private const int PieceRadius = 3;

    private readonly LedFont font;

    private CheckerPiece[,] board;
    private CheckerPiece selectedPiece;
    private bool isPlayer1Turn;
    private int cursorX;
    private int cursorY;

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

        var newX = cursorX + dx;
        var newY = cursorY + dy;

        if (newX >= 0 && newX < BoardSize && newY >= 0 && newY < BoardSize)
        {
            cursorX = newX;
            cursorY = newY;
        }

        if (buttons == Buttons.Green)
        {
            var piece = board[cursorX, cursorY];
            if (piece != null && piece.IsPlayer1 == isPlayer1)
            {
                selectedPiece = piece;
            }
            else if (selectedPiece != null)
            {
                TryToMovePiece(isPlayer1);
            }
        }
        else if (buttons == Buttons.Blue)
        {
            selectedPiece = null;
        }
    }

    private void TryToMovePiece(bool isPlayer1)
    {
        int dxMove = Math.Abs(cursorX - selectedPiece.X);
        int dyMove = isPlayer1 ? cursorY - selectedPiece.Y : selectedPiece.Y - cursorY;
        bool validMove = (dxMove == 1 && dyMove == 1) || (dxMove == 2 && dyMove == 2);

        if (validMove)
        {
            bool isCaptureMove = dxMove == 2;
            int captureX = (cursorX + selectedPiece.X) / 2;
            int captureY = (cursorY + selectedPiece.Y) / 2;

            if (isCaptureMove && board[captureX, captureY]?.IsPlayer1 != isPlayer1)
            {
                board[captureX, captureY] = null; // Remove the captured piece
            }
            else if (isCaptureMove)
            {
                // Invalid capture move
                return;
            }

            board[selectedPiece.X, selectedPiece.Y] = null;
            selectedPiece.X = cursorX;
            selectedPiece.Y = cursorY;
            board[cursorX, cursorY] = selectedPiece;

            if (cursorY == 0 || cursorY == BoardSize - 1)
            {
                selectedPiece.IsKing = true;
            }

            if (isCaptureMove && CanCaptureAgain(selectedPiece, isPlayer1))
            {
                // Allow multiple captures
                return;
            }

            selectedPiece = null;
            isPlayer1Turn = !isPlayer1Turn;

            if (IsGameOver())
            {
                Console.WriteLine("Game over!");
            }
        }
    }

    private bool CanCaptureAgain(CheckerPiece piece, bool isPlayer1)
    {
        int[] directions = { -1, 1 };
        foreach (int dx in directions)
        {
            foreach (int dy in directions)
            {
                int x = piece.X + dx * 2;
                int y = piece.Y + (isPlayer1 ? -dy * 2 : dy * 2);
                int captureX = piece.X + dx;
                int captureY = piece.Y + (isPlayer1 ? -dy : dy);

                if (x >= 0 && x < BoardSize && y >= 0 && y < BoardSize)
                {
                    CheckerPiece targetPiece = board[x, y];
                    CheckerPiece capturePiece = board[captureX, captureY];

                    if (targetPiece == null && capturePiece?.IsPlayer1 != isPlayer1)
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }

    private bool IsGameOver()
    {
        int player1Pieces = 0;
        int player2Pieces = 0;

        for (int y = 0; y < BoardSize; y++)
        {
            for (int x = 0; x < BoardSize; x++)
            {
                CheckerPiece piece = board[x, y];
                if (piece != null)
                {
                    if (piece.IsPlayer1)
                    {
                        player1Pieces++;
                    }
                    else
                    {
                        player2Pieces++;
                    }
                }
            }
        }

        return player1Pieces == 0 || player2Pieces == 0;
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
                    var pieceColor = piece.IsPlayer1 ? Color.DarkRed : Color.DarkBlue;
                    display.DrawCircle(xPos + CellSize / 2, yPos + CellSize / 2, PieceRadius, pieceColor, pieceColor);
                    if (piece == selectedPiece)
                    {
                        pieceColor = piece.IsPlayer1 ? Color.Red : Color.Blue;
                        display.DrawCircle(xPos + CellSize / 2, yPos + CellSize / 2, PieceRadius, pieceColor, pieceColor);
                    }
                }
            }
        }

        // cursor
        display.DrawRectangle(cursorX * CellSize, cursorY * CellSize, CellSize, CellSize, Color.Lime);
        // Draw the player's turn
        var playerTurnText = isPlayer1Turn ? "Player 1" : "Player 2";
        var turnTextColor = isPlayer1Turn ? Color.Red : Color.Blue;
        font.DrawText(display, 1, 35, turnTextColor, playerTurnText);
    }

    public GameOverState State => GameOverState.None;
}