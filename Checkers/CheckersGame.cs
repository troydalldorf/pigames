using System.Drawing;
using Checkers.Bits;
using Core;
using Core.Display;
using Core.Display.Sprites;
using Core.Fonts;

namespace Checkers;

public class CheckersGame : IDuoPlayableGameElement
{
    private const int BoardSize = 8;
    private const int CellSize = 8;
    private const int Padding = 0;

    private CheckerPiece[,] board;
    private CheckerPiece selectedPiece;
    private bool isPlayer1Turn;
    private int cursorX;
    private int cursorY;
    private readonly ISprite pieces;

    public CheckersGame(IFontFactory fontFactory)
    {
        var image = SpriteImage.FromResource("c4.png", new Point(1, 1));
        pieces = image.GetSpriteAnimation(1, 1, 8, 8, 5, 1);
        
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
                        board[x, y] = new CheckerPiece(false, false, x, y);
                    }
                    else if (y > 4)
                    {
                        board[x, y] = new CheckerPiece(true, false, x, y);
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
        int dyMove = cursorY - selectedPiece.Y;
        bool simpleMove = (dxMove == 1 && Math.Abs(dyMove) == 1 && board[cursorX, cursorY] == null) && 
                          ((isPlayer1 && dyMove < 0) || (!isPlayer1 && dyMove > 0));
        bool validMove = simpleMove || (dxMove == 2 && Math.Abs(dyMove) == 2);

        if (validMove)
        {
            bool isCaptureMove = dxMove == 2;
            int captureX = (cursorX + selectedPiece.X) / 2;
            int captureY = (cursorY + selectedPiece.Y) / 2;

            if (isCaptureMove && board[captureX, captureY]?.IsPlayer1 != isPlayer1)
            {
                board[captureX, captureY] = null; // Remove the captured piece
                selectedPiece.HasCaptured = true; // Set flag for multi-jump move
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

            // Reset the HasCaptured flag if the piece did not make a capture
            if (!isCaptureMove)
            {
                selectedPiece.HasCaptured = false;
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
                int y = piece.Y + dy * 2;
                int captureX = piece.X + dx;
                int captureY = piece.Y + dy;

                if (x >= 0 && x < BoardSize && y >= 0 && y < BoardSize)
                {
                    CheckerPiece targetPiece = board[x, y];
                    CheckerPiece capturePiece = board[captureX, captureY];

                    if (targetPiece == null && capturePiece?.IsPlayer1 != isPlayer1)
                    {
                        if (piece.IsKing == true || (isPlayer1 && dy > 0) || (!isPlayer1 && dy < 0))
                        {
                            return true;
                        }
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

                var cellColor = (x + y) % 2 == 0 ? Color.Black : Color.FromArgb(20, 20, 20);
                display.DrawRectangle(xPos, yPos, CellSize, CellSize, cellColor, cellColor);
                
                // cursor
                if (x == cursorX && y == cursorY)
                {
                    var cursorColor = isPlayer1Turn ? Color.Red : Color.Blue;
                    display.DrawRectangle(cursorX * CellSize, cursorY * CellSize, CellSize, CellSize, cursorColor);
                }

                var piece = board[x, y];
                if (piece != null)
                {
                    var pieceColor = (piece.IsPlayer1 ? 2 : 1) + (selectedPiece == piece ? 2 : 0);
                    this.pieces.Draw(display, xPos, yPos, pieceColor);
                }
            }
        }
    }

    public GameOverState State => GameOverState.None;
}