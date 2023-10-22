using System.Drawing;
using Core;
using Core.Display;
using Stacker.Bits;

namespace Stacker;

public class StackerGame2P : IDuoPlayableGameElement
{
    private const int MaxStack = 6;
    private readonly Player player1;
    private readonly Player player2;

    public StackerGame2P()
    {
        var config = new Config(new Size(4, 4), 4, Color.Fuchsia, 64, 64);
        player1 = new Player(config);
        player2 = new Player(config with { Color = Color.Aqua });
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
        if (player1.IsDone || player2.IsDone || player1.StackSize + player2.StackSize > MaxStack * 2)
        {
            if (player1.Score == player2.Score)
                this.State = GameOverState.Draw;
            else if (player1.Score > player2.Score)
                this.State = GameOverState.Player1Wins;
            else if (player2.Score > player1.Score)
                this.State = GameOverState.Player2Wins;
        }
    }

    public void Draw(IDisplay display)
    {
        player1.Draw(display);
        var p2Display = new TxDisplay(display, (x, _) => 63 - x, (_, y) => 63 - y);
        player2.Draw(p2Display);
    }

    public GameOverState State { get; private set; }
}