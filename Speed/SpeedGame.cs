using Core;
using Core.Fonts;
using Speed.Bits;
using System.Drawing;

namespace Speed;

public class SpeedGame : IDuoPlayableGameElement
{
    private const int TimeoutSeconds = 5;
    private DateTime timeout = DateTime.Now.AddSeconds(TimeoutSeconds);
    private readonly Player p1;
    private readonly Player p2;
    private readonly IFont font;
    private bool matched = false;

    public SpeedGame(IFontFactory fontFactory)
    {
        font = fontFactory.GetFont(LedFontType.Font6x12);
        var deck = Card.GenerateDeck().Shuffle().ToList();
        p1 = new Player(deck.Take(26));
        p2 = new Player(deck.Skip(26).Take(26));
        p1.IsTurn = true;
        p2.IsTurn = false;
    }

    public void HandleInput(IPlayerConsole player1Console)
    {
        HandleInput(player1Console, p1, p2, GameOverState.Player1Wins);
    }

    public void Handle2PInput(IPlayerConsole player2Console)
    {
        HandleInput(player2Console, p2, p1, GameOverState.Player2Wins);
    }
    
    public void HandleInput(IPlayerConsole console, Player player, Player otherPlayer, GameOverState winState)
    {
        var buttons = console.ReadButtons();
        if (player.IsTurn && buttons.IsBluePushed())
        {
            TryNextCard(player, otherPlayer, winState);
        }
        else if (!player.IsTurn && buttons.IsBluePushed())
        {
            player.ScoreMiss();
        }

        if (buttons.IsGreenPushed())
        {
            CheckMatch(1);
        }
    }

    private void TryNextCard(Player player, Player otherPlayer, GameOverState winState)
    {
        if (player.HasCards) 
        {
            player.NextCard();
            player.IsTurn = false;
            otherPlayer.IsTurn = true;
            timeout = DateTime.Now.AddSeconds(TimeoutSeconds);
        }
        else
        {
            State = winState;
        }
        matched = false;
    }

    public void Update()
    {
        if (p1.IsTurn && DateTime.Now >= timeout) 
            TryNextCard(p1, p2, GameOverState.Player1Wins);

        if (p2.IsTurn && DateTime.Now >= timeout)
            TryNextCard(p2, p1, GameOverState.Player2Wins);
    }

    public void Draw(IDisplay display)
    {
        display.Clear();
        font.DrawText(display, 16, 48, Color.White, p1.Score.ToString());
        p1.CurrentCard?.Draw(display, display.Width - Card.CardWidth, display.Height -Card.CardHeight, p1.IsTurn ? Color.Red : null, font);
        p2.CurrentCard?.Draw(display, 0, 0, p2.IsTurn ? Color.Blue : null, font);
        font.DrawText(display, 48, 16, Color.White, p2.Score.ToString());
    }

    public GameOverState State { get; private set; }

    private void CheckMatch(int player)
    {
        if (matched) return;
        if (p1.CurrentCard != null && p1.CurrentCard?.Rank == p2.CurrentCard?.Rank)
        {
            matched = true;
            if (player == 1)
                p1.ScoreMatch();
            else 
                p2.ScoreMatch();
        }
    }
}