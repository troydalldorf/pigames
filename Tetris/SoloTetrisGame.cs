using Core.Display;
using Core.Inputs;

namespace Tetris;

public class SoloTetrisGame
{
    private readonly LedDisplay display;
    private readonly PlayerConsole playerConsole;
    private readonly TetrisGame tetrisGame;

    public SoloTetrisGame(LedDisplay display, PlayerConsole playerConsole)
    {
        this.display = display;
        this.playerConsole = playerConsole;
        this.tetrisGame = new TetrisGame();
    }

    public void Run()
    {
        while (true)
        {
            tetrisGame.HandleInput(playerConsole);
            tetrisGame.Update();
            tetrisGame.Draw(display);
            Thread.Sleep(50);
        }
    }
}