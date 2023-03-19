using Core;
using Core.Display;
using Core.Inputs;

namespace Tetris;

public class SoloTetrisGame : IGameElement
{
    private readonly LedDisplay display;
    private readonly PlayerConsole playerConsole;
    private readonly TetrisGame tetrisGame;

    public SoloTetrisGame()
    {
        this.tetrisGame = new TetrisGame();
    }

    public void Run(LedDisplay display, PlayerConsole playerConsole)
    {
        while (true)
        {
            tetrisGame.HandleInput(playerConsole);
            tetrisGame.Update();
            tetrisGame.Draw(display);
            Thread.Sleep(50);
        }
    }

    public void HandleInput(IPlayerConsole player1Console)
    {
        tetrisGame.HandleInput(player1Console);
    }

    public void Update()
    {
        tetrisGame.Update();
    }

    public void Draw(IDisplay display)
    {
        tetrisGame.Draw(display);
    }
}