using System.Drawing;
using Core;
using Core.Display.Sprites;

namespace Chess;

// TO DO:
// 1. Prevent cursor from moving outside the board
// 2. cursor should be the color of the current player
// 3. Implement piece movement - check for valid moves
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
    public ChessGame()
    {
        var image = SpriteImage.FromResource("chess.png", new Point(1, 1));
        whitePieces = image.GetSpriteAnimation(10, 1, 8, 8, 6, 1);
        blackPieces = image.GetSpriteAnimation(10, 10, 8, 8, 6, 1);
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
        display.DrawRectangle(cursorX * 8, cursorY * 8, 8, 8, Color.Peru);
    }

    public void HandleInput(IPlayerConsole player1Console)
    {
        var stick = player1Console.ReadJoystick();
        var buttons = player1Console.ReadButtons();
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
            if (selectedX == -1)
            {
                selectedX = cursorX;
                selectedY = cursorY;
            }
            else
            {
                if (board[selectedX, selectedY] != null)
                {
                    board[cursorX, cursorY] = board[selectedX, selectedY];
                    board[selectedX, selectedY] = null;
                }
                selectedX = -1;
                selectedY = -1;
            }
        }
    }
    
    public void Handle2PInput(IPlayerConsole player2Console)
    {
    }
    
    public GameOverState State { get; }
}