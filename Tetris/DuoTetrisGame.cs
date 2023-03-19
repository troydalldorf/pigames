using Core;
using Core.Effects;

namespace Tetris;

public class DuoTetrisGame : I2PGameElement
{
    private readonly TetrisGame p1TetrisGame;
    private readonly TetrisGame p2TetrisGame;

    public DuoTetrisGame()
    {
        this.p1TetrisGame = new TetrisGame();
        this.p2TetrisGame = new TetrisGame();
    }

    public void Run(IDisplay display, IPlayerConsole p1Console, IPlayerConsole p2Console)
    {
        while (true)
        {
            this.HandleInput(p1Console);
            this.HandleInput(p2Console);
            this.Update();
            display.Clear();
            this.Draw(display);
            display.Update();
            Thread.Sleep(50);
        }
    }

    public void HandleInput(IPlayerConsole p1Console)
    {
        p1TetrisGame.HandleInput(p1Console);
    }

    public void Handle2PInput(IPlayerConsole p2Console)
    {
        p2TetrisGame.HandleInput(p2Console);
    }

    public void Update()
    {
        p1TetrisGame.Update();
        p2TetrisGame.Update();
    }

    public void Draw(IDisplay display)
    {
        var p2Display = new TxDisplay(display, (x, _) => 63 - x, (_, y) => 63 - y);
        display.Clear();
        p1TetrisGame.Draw(display);
        p2TetrisGame.Draw(p2Display);
        display.Update();
    }

    public bool IsDone()
    {
        return p1TetrisGame.IsDone() && p2TetrisGame.IsDone();
    }
}