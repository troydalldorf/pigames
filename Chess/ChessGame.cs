using System.Drawing;
using Core;
using Core.Display.Sprites;

namespace Chess;

public class ChessGame : IDuoPlayableGameElement
{
    private SpriteAnimation whitePieces;
    private SpriteAnimation blackPieces;
    
    public ChessGame()
    {
        var image = SpriteImage.FromResource("chess.png");
        whitePieces = image.GetSpriteAnimation(9, 1, 8, 8, 6, 1)
        State = GameOverState.None;
        blackPieces = image.GetSpriteAnimation(9, 9, 8, 8, 6, 1)
        State = GameOverState.None;
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