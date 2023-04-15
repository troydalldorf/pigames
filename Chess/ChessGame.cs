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
        board[0, 0] = Piece.Rook;
        board[1, 0] = Piece.Knight;
        board[2, 0] = Piece.Bishop;
        board[3, 0] = Piece.Queen;
        board[4, 0] = Piece.King;
        board[5, 0] = Piece.Bishop;
        board[6, 0] = Piece.Knight;
        board[7, 0] = Piece.Rook;
        for (int i = 0; i < 8; i++)
        {
            board[i, 1] = Piece.Pawn;
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
                display.DrawRectangle(i*8, j*8,8,8, color,color);
                if (board[i, j] != Piece.None)
                {
                   whitePieces.Draw(display, i*8, j*8, (int)board[i, j] - 1);
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