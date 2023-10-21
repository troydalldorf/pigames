using Core;
using Core.Display;
using Core.Effects;
using Core.Fonts;

namespace Tetris;

public class DuoTetrisGame : IDuoPlayableGameElement
{
    private readonly TetrisGame p1TetrisGame;
    private readonly TetrisGame p2TetrisGame;

    public DuoTetrisGame(IFontFactory fontFactory)
    {
        this.p1TetrisGame = new TetrisGame(fontFactory);
        this.p2TetrisGame = new TetrisGame(fontFactory);
    }

    public void HandleInput(IPlayerConsole p1Console)
    {
        p1TetrisGame.HandleInput(p1Console);
    }

    public void Handle2PInput(IPlayerConsole player2Console)
    {
        p2TetrisGame.HandleInput(player2Console);
    }

    public void Update()
    {
        p1TetrisGame.Update();
        p2TetrisGame.Update();
    }

    public void Draw(IDisplay display)
    {
        var p2Display = new TxDisplay(display, (x, _) => 63 - x, (_, y) => 63 - y);
        p1TetrisGame.Draw(display);
        p2TetrisGame.Draw(p2Display);
    }

    public GameOverState State
    {
        get
        {
            if (p1TetrisGame.State == GameOverState.EndOfGame && p2TetrisGame.State == GameOverState.EndOfGame)
                return GameOverState.Draw;
            if (p1TetrisGame.State == GameOverState.EndOfGame)
                return GameOverState.Player2Wins;
            if (p2TetrisGame.State == GameOverState.EndOfGame)
                return GameOverState.Player1Wins;
            return GameOverState.None;
        }
    }
}