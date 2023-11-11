using Core;
using Core.Display;
using Core.Effects;
using Core.Fonts;
using Core.Inputs;
using Core.State;

namespace Mastermind;

public class DuoMastermindGame : IDuoPlayableGameElement
{
    private readonly MastermindGame p1Game;
    private readonly MastermindGame p2Game;

    public DuoMastermindGame(IFontFactory fontFactory)
    {
        this.p1Game = new MastermindGame(fontFactory);
        this.p2Game = new MastermindGame(fontFactory);
    }

    public void HandleInput(IPlayerConsole p1Console)
    {
        p1Game.HandleInput(p1Console);
    }

    public void Handle2PInput(IPlayerConsole player2Console)
    {
        p2Game.HandleInput(player2Console);
    }

    public void Update()
    {
        p1Game.Update();
        p2Game.Update();
    }

    public void Draw(IDisplay display)
    {
        var p2Display = new TxDisplay(display, (x, _) => 61 - x, (_, y) => 61 - y);
        p1Game.Draw(display);
        p2Game.Draw(p2Display);
    }

    public GameOverState State
    {
        get
        {
            if (p1Game.State == GameOverState.EndOfGame && p2Game.State == GameOverState.EndOfGame)
                return GameOverState.Draw;
            if (p1Game.State == GameOverState.EndOfGame)
                return GameOverState.Player2Wins;
            if (p2Game.State == GameOverState.EndOfGame)
                return GameOverState.Player1Wins;
            return GameOverState.None;
        }
    }
}