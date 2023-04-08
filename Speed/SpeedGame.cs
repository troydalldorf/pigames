using Core;
using Core.Fonts;
using Speed.Bits;

namespace Speed;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

public class SpeedGame : IDuoPlayableGameElement
{
    private const int TimeoutSeconds = 5;

    private readonly Queue<Card> player1Cards;
    private readonly Queue<Card> player2Cards;
    private Card? player1Card;
    private Card? player2Card;
    private int player1Score;
    private int player2Score;
    private DateTime player1Timeout;
    private DateTime player2Timeout;
    private bool p1Turn;
    private readonly IFont font;

    public SpeedGame(IFontFactory fontFactory)
    {
        font = fontFactory.GetFont(LedFontType.Font6x12);

        var deck = Card.GenerateDeck().Shuffle().ToList();
        player1Cards = new Queue<Card>(deck.Take(26));
        player2Cards = new Queue<Card>(deck.Skip(26).Take(26));

        player1Score = 0;
        player2Score = 0;

        player1Card = null;
        player2Card = null;

        player1Timeout = DateTime.Now.AddSeconds(TimeoutSeconds);
        player2Timeout = DateTime.Now.AddSeconds(TimeoutSeconds);
        p1Turn = true;
    }

    public void HandleInput(IPlayerConsole player1Console)
    {
        var buttons = player1Console.ReadButtons();
        if (p1Turn && buttons.IsBluePushed())
        {
            if (player1Cards.Count > 0)
            {
                player1Card = player1Cards.Dequeue();
                p1Turn = false;
                player2Timeout = DateTime.Now.AddSeconds(TimeoutSeconds);
            }
            else
            {
                State = GameOverState.Player2Wins;
            }
        }
        else if (!p1Turn && buttons.IsBluePushed())
        {
            player1Score--;
        }

        if (buttons.IsGreenPushed())
        {
            CheckMatch(1);
        }
    }

    public void Handle2PInput(IPlayerConsole player2Console)
    {
        var buttons = player2Console.ReadButtons();
        if (!p1Turn && buttons.IsBluePushed())
        {
            if (player2Cards.Count > 0)
            {
                player2Card = player2Cards.Dequeue();
                p1Turn = true;
                player1Timeout = DateTime.Now.AddSeconds(TimeoutSeconds);
            }
            else
            {
                State = GameOverState.Player1Wins;
            }
        }
        else if (p1Turn && buttons.IsBluePushed())
        {
            player1Score--;
        }

        if (buttons.IsGreenPushed())
        {
            CheckMatch(2);
        }
    }

    public void Update()
    {
        if (p1Turn && DateTime.Now >= player1Timeout)
        {
            if (player1Cards.Count > 0)
            {
                player1Card = player1Cards.Dequeue();
            }
            p1Turn = false;
            player2Timeout = DateTime.Now.AddSeconds(TimeoutSeconds);
        }

        if (!p1Turn && DateTime.Now >= player2Timeout)
        {
            if (player2Cards.Count > 0)
            {
                player2Card = player2Cards.Dequeue();
            }

            p1Turn = true;
            player2Timeout = DateTime.Now.AddSeconds(TimeoutSeconds);
        }
    }

    public void Draw(IDisplay display)
    {
        display.Clear();
        player1Card?.Draw(display, 0, 0, Color.Green, font);
        player2Card?.Draw(display, display.Width - Card.CardWidth, display.Height -Card.CardHeight, Color.Green, font);
        font.DrawText(display, 2, display.Height / 2 - 7, Color.White, player1Score.ToString());
        font.DrawText(display, display.Width - 10, display.Height / 2 + 2, Color.White, player2Score.ToString());
    }

    public GameOverState State { get; private set; }

    private void CheckMatch(int player)
    {
        if (player1Card != null && player2Card != null && player1Card.Rank == player2Card.Rank)
        {
            if (player == 1)
            {
                player1Score++;
            }
            else
            {
                player2Score++;
            }
        }
    }
}