using System.Drawing;
using Core;
using Core.Display;
using Stacker.Bits;

namespace Stacker;

public class StackerGame2P : IDuoPlayableGameElement
{
    private int MaxStack = 7;
    private readonly Player player1 = new(new Size(4, 4), Color.Fuchsia, 64);
    private readonly Player player2 = new(new Size(4, 4), Color.Aqua, 64);
    
    public StackerGame2P()
    {
        State = GameOverState.None;
    }

    public void HandleInput(IPlayerConsole player1Console)
    {
        HandleInput(player1, player1Console);
    }

    public void Handle2PInput(IPlayerConsole player2Console)
    {
        HandleInput(player2, player2Console);
    }

    private void HandleInput(Player player, IPlayerConsole console)
    {
        var buttons = console.ReadButtons();
        if (buttons.IsGreenPushed())
        {
            player.Next();
        }
    }

    public void Update()
    {
        player1.Update();
        player2.Update();
        if (player1.IsDone && player2.IsDone)
            this.State = GameOverState.Draw;
        else if (player1.IsDone)
            this.State = GameOverState.Player2Wins;
        else if (player2.IsDone)
            this.State = GameOverState.Player1Wins;
        if (player1.StackSize >= MaxStack && player2.StackSize >= MaxStack)
            this.State = GameOverState.Draw;
        else if (player1.StackSize > MaxStack)
            this.State = GameOverState.Player1Wins;
        else if (player2.StackSize > MaxStack)
            this.State = GameOverState.Player2Wins;
    }

    public void Draw(IDisplay display)
    {
        player1.Draw(display);
        var p2Display = new TxDisplay(display, (x, _) => 63 - x, (_, y) => 63 - y);
        player2.Draw(p2Display);
    }

    public GameOverState State { get; private set; }
}