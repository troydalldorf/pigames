using Core;
using Core.Effects;

namespace Tetris;

public class DuoPlayableTetrisGame : IDuoPlayableGameElement
{
    private readonly TetrisPlayableGame p1TetrisPlayableGame;
    private readonly TetrisPlayableGame p2TetrisPlayableGame;

    public DuoPlayableTetrisGame()
    {
        this.p1TetrisPlayableGame = new TetrisPlayableGame();
        this.p2TetrisPlayableGame = new TetrisPlayableGame();
    }

    public void HandleInput(IPlayerConsole p1Console)
    {
        p1TetrisPlayableGame.HandleInput(p1Console);
    }

    public void Handle2PInput(IPlayerConsole player2Console)
    {
        p2TetrisPlayableGame.HandleInput(player2Console);
    }

    public void Update()
    {
        p1TetrisPlayableGame.Update();
        p2TetrisPlayableGame.Update();
    }

    public void Draw(IDisplay display)
    {
        var p2Display = new TxDisplay(display, (x, _) => 61 - x, (_, y) => 61 - y);
        p1TetrisPlayableGame.Draw(display);
        p2TetrisPlayableGame.Draw(p2Display);
    }

    public GameOverState State
    {
        get
        {
            if (p1TetrisPlayableGame.State == GameOverState.EndOfGame && p2TetrisPlayableGame.State == GameOverState.EndOfGame)
                return GameOverState.Draw;
            if (p1TetrisPlayableGame.State == GameOverState.EndOfGame)
                return GameOverState.Player2Wins;
            if (p2TetrisPlayableGame.State == GameOverState.EndOfGame)
                return GameOverState.Player1Wins;
            return GameOverState.None;
        }
    }
}