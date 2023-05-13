using System.Drawing;
using Core;
using Core.Display.Sprites;

namespace Chess;

// TO DO:
// 4. Implement checkmate
// 5. Implement castling
// 6. Implement en passant
// 7. Implement pawn promotion
// 8. Implement stalemate
// 9. Implement draw by repetition
// 10. Implement draw by 50 move rule
// 11. Implement draw by insufficient material
// 13. Implement draw by 5-fold repetition

public class ChessGame : IDuoPlayableGameElement
{
    private int cursorX = 4;
    private int cursorY = 4;
    private SpriteAnimation whitePieces;
    private SpriteAnimation blackPieces;
    private Piece[,] board = new Piece[8, 8];
    private int selectedX = -1;
    private int selectedY = -1;
    private PieceColor currentPlayer = PieceColor.White;
    private DateTimeOffset lastMoveTime = DateTimeOffset.Now;
    public ChessGame()
    {
        var image = SpriteImage.FromResource("chess.png", new Point(1, 1));
        blackPieces = image.GetSpriteAnimation(10, 1, 8, 8, 6, 1);
        whitePieces = image.GetSpriteAnimation(10, 10, 8, 8, 6, 1);
        State = GameOverState.None;
        board[0, 0] = new Piece(PieceType.Rook, PieceColor.Black);
        board[1, 0] = new Piece(PieceType.Knight, PieceColor.Black);
        board[2, 0] = new Piece(PieceType.Bishop, PieceColor.Black);
        board[3, 0] = new Piece(PieceType.Queen, PieceColor.Black);
        board[4, 0] = new Piece(PieceType.King, PieceColor.Black);
        board[5, 0] = new Piece(PieceType.Bishop, PieceColor.Black);
        board[6, 0] = new Piece(PieceType.Knight, PieceColor.Black);
        board[7, 0] = new Piece(PieceType.Rook, PieceColor.Black);
        for (int i = 0; i < 8; i++)
        {
            board[i, 1] = new Piece(PieceType.Pawn, PieceColor.Black);
        }
        board[0, 7] = new Piece(PieceType.Rook, PieceColor.White);
        board[1, 7] = new Piece(PieceType.Knight, PieceColor.White);
        board[2, 7] = new Piece(PieceType.Bishop, PieceColor.White);
        board[3, 7] = new Piece(PieceType.Queen, PieceColor.White);
        board[4, 7] = new Piece(PieceType.King, PieceColor.White);
        board[5, 7] = new Piece(PieceType.Bishop, PieceColor.White);
        board[6, 7] = new Piece(PieceType.Knight, PieceColor.White);
        board[7, 7] = new Piece(PieceType.Rook, PieceColor.White);
        for (int i = 0; i < 8; i++)
        {
            board[i, 6] = new Piece(PieceType.Pawn, PieceColor.White);
        }
    }
    
    public void Update()
    {
    }

    public void Draw(IDisplay display)
    {
        var cursorColor = currentPlayer == PieceColor.White ? Color.SpringGreen : Color.Magenta;
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                Color color;
                if ((i + j) % 2 == 0)
                {
                    color = Color.Black;
                }
                else
                {
                    color = Color.White;
                }
                if (selectedX == i && selectedY == j)
                {
                    color = cursorColor;
                }

                display.DrawRectangle(i * 8, j * 8, 8, 8, color, color);
                if (board[i, j] != null && board[i, j].Color == PieceColor.Black)
                {
                    blackPieces.Draw(display, i * 8, j * 8, (int)board[i, j].Type);
                }
                else if (board[i, j] != null && board[i, j].Color == PieceColor.White)

                {
                    whitePieces.Draw(display, i * 8, j * 8, (int)board[i, j].Type);
                }
            }
        }
        display.DrawRectangle(cursorX * 8, cursorY * 8, 8, 8, cursorColor);
    }

    public void HandleInput(IPlayerConsole player1Console)
    {
        if (currentPlayer == PieceColor.White)
        {
            HandlePlayerInput(player1Console);
        }
    }

    public void Handle2PInput(IPlayerConsole player2Console)
    {
        if (currentPlayer == PieceColor.Black)
        {
            HandlePlayerInput(player2Console);
        }
    }
    
    public void HandlePlayerInput(IPlayerConsole console)
    {
        // Prevent trigger happy players from moving the cursor too fast
        if (lastMoveTime.AddMilliseconds(200) > DateTimeOffset.Now)
        {
            return;
        }
        
        var stick = console.ReadJoystick();
        var buttons = console.ReadButtons();
        if (stick.IsUp())
        {
            cursorY--;
        } 
        if (stick.IsDown())
        {
            cursorY++;
        } 
        if (stick.IsLeft())
        {
            cursorX--;
        } 
        if (stick.IsRight())
        {
            cursorX++;
        }
        // prevent cursor from moving outside the board
        if (cursorX < 0)
        {
            cursorX = 0;
        }
        if (cursorX > 7)
        {
            cursorX = 7;
        }
        if (cursorY < 0)
        {
            cursorY = 0;
        }
        if (cursorY > 7)
        {
            cursorY = 7;
        }
        if (buttons.IsGreenPushed())
        {
            if (selectedX == -1 && board[cursorX, cursorY] != null && board[cursorX, cursorY].Color == currentPlayer)
            {
                selectedX = cursorX;
                selectedY = cursorY;
            }
            else if (selectedX != -1)
            {
                if (IsValidMove(selectedX, selectedY, cursorX, cursorY))
                {
                    board[cursorX, cursorY] = board[selectedX, selectedY] with { HasMoved = true };
                    board[selectedX, selectedY] = null;
                    currentPlayer = currentPlayer == PieceColor.White ? PieceColor.Black : PieceColor.White;
                }
                selectedX = -1;
                selectedY = -1;
            }
        }
    }

private bool IsValidMove(int startX, int startY, int endX, int endY)
{
    var currentPiece = board[startX, startY];
    var targetPiece = board[endX, endY];

    if (currentPiece == null)
    {
        return false;
    }

    if (targetPiece != null && targetPiece.Color == currentPiece.Color)
    {
        return false;
    }

    int deltaX = Math.Abs(endX - startX);
    int deltaY = Math.Abs(endY - startY);

    switch (currentPiece.Type)
    {
        case PieceType.Pawn:
            // Pawns can only move forward one space (two on their first move), 
            // and can only capture diagonally
            if (targetPiece == null)
            {
                if (currentPiece.Color == PieceColor.White)
                {
                    if (deltaX == 0 && (deltaY == 1 || (!currentPiece.HasMoved && deltaY == 2)) && endY < startY)
                    {
                        return true;
                    }
                }
                else
                {
                    if (deltaX == 0 && (deltaY == 1 || (!currentPiece.HasMoved && deltaY == 2)) && endY > startY)
                    {
                        return true;
                    }
                }
            }
            else
            {
                if (deltaX == 1 && deltaY == 1)
                {
                    return true;
                }
            }
            break;
        case PieceType.Rook:
            // Rooks can move any number of spaces along any row or column
            if (deltaX * deltaY == 0)
            {
                return true;
            }
            break;
        case PieceType.Knight:
            // Knights move in an L shape: two spaces along a row or column, and then one space perpendicular
            if (deltaX * deltaY == 2)
            {
                return true;
            }
            break;
        case PieceType.Bishop:
            // Bishops move any number of spaces diagonally
            if (deltaX == deltaY)
            {
                return true;
            }
            break;
        case PieceType.Queen:
            // Queens can move any number of spaces along any row, column, or diagonal
            if (deltaX * deltaY == 0 || deltaX == deltaY)
            {
                return true;
            }
            break;
        case PieceType.King:
            // Kings can move one space in any direction
            if (deltaX <= 1 && deltaY <= 1)
            {
                return true;
            }
            break;
    }

    return false;
}

public GameOverState State { get; }
}