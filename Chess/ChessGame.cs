using System.Drawing;
using Core;
using Core.Display.Sprites;

namespace Chess;

public class ChessGame : IDuoPlayableGameElement
{
    private SpriteAnimation whitePieces;
    private SpriteAnimation blackPieces;
    private Piece[,] board = new Piece[8, 8];
    public ChessGame()
    {
        var image = SpriteImage.FromResource("chess.png");
        whitePieces = image.GetSpriteAnimation(9, 1, 8, 8, 6, 1);
        blackPieces = image.GetSpriteAnimation(9, 9, 8, 8, 6, 1);
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
    }

    public void HandleInput(IPlayerConsole player1Console)
    {
    }
    
    public void Handle2PInput(IPlayerConsole player2Console)
    {
    }
    
    public GameOverState State { get; }
}