using System.Drawing;
using Core;

namespace Chess;

public class ChessGame : IDuoPlayableGameElement
{
    public ChessGame()
    {
        
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
        throw new NotImplementedException();
    }
    public GameOverState State { get; }
}