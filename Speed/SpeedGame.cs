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
    private bool player1Ready;
    private bool player2Ready;
    private readonly IFont font;

    public SpeedGame(IFontFactory fontFactory)
    {
        font = fontFactory.GetFont(LedFontType.Font5x7);

        var deck = Card.GenerateDeck().Shuffle().ToList();
        player1Cards = new Queue<Card>(deck.Take(26));
        player2Cards = new Queue<Card>(deck.Skip(26).Take(26));

        player1Score = 0;
        player2Score = 0;

        player1Card = null;
        player2Card = null;

        player1Timeout = DateTime.Now.AddSeconds(TimeoutSeconds);
        player2Timeout = DateTime.Now.AddSeconds(TimeoutSeconds);

        player1Ready = false;
        player2Ready = false;
    }

    public void HandleInput(IPlayerConsole player1Console)
    {
        var buttons = player1Console.ReadButtons();

        if ((buttons & Buttons.Blue) != 0)
        {
            player1Ready = true;
        }

        if ((buttons & Buttons.Green) != 0)
        {
            CheckMatch(1);
        }
    }

    public void Handle2PInput(IPlayerConsole player2Console)
    {
        var buttons = player2Console.ReadButtons();

        if ((buttons & Buttons.Blue) != 0)
        {
            player2Ready = true;
        }

        if ((buttons & Buttons.Green) != 0)
        {
            CheckMatch(2);
        }
    }

    public void Update()
    {
        if (player1Ready || DateTime.Now >= player1Timeout)
        {
            if (player1Cards.Count > 0)
            {
                player1Card = player1Cards.Dequeue();
            }

            player1Timeout = DateTime.Now.AddSeconds(TimeoutSeconds);
            player1Ready = false;
        }

        if (player2Ready || DateTime.Now >= player2Timeout)
        {
            if (player2Cards.Count > 0)
            {
                player2Card = player2Cards.Dequeue();
            }

            player2Timeout = DateTime.Now.AddSeconds(TimeoutSeconds);
            player2Ready = false;
        }
    }

    public void Draw(IDisplay display)
    {
        display.Clear();

        // Draw Player 1's card
        if (player1Card != null)
        {
            player1Card.Draw(display, 0, 0, true, font);
        }

        // Draw Player 2's card
        if (player2Card != null)
        {
            player2Card.Draw(display, display.Width - Card.CardWidth, display.Height -Card.CardHeight, false, font);
        }

        // Draw scores
        font.DrawText(display, 2, display.Height / 2 - 7, Color.White, player1Score.ToString());
        font.DrawText(display, display.Width - 10, display.Height / 2 + 2, Color.White, player2Score.ToString());
    }

    public GameOverState State => GameOverState.None;

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

            player1Card = null;
            player2Card = null;
        }
        else
        {
            if (player == 1)
            {
                player1Score--;
            }
            else
            {
                player2Score--;
            }
        }
    }
}