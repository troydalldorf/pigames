using System.Drawing;
using Core;
using Core.Fonts;

namespace Speed.Bits;

public class Card
{
    public static int CardWidth = 24;
    public static int CardHeight = 34;
    public Suit Suit { get; }
    public Rank Rank { get; }

    public Card(Suit suit, Rank rank)
    {
        Suit = suit;
        Rank = rank;
    }

    public static List<Card> GenerateDeck()
    {
        var deck = new List<Card>();
        foreach (Suit suit in Enum.GetValues(typeof(Suit)))
        {
            foreach (Rank rank in Enum.GetValues(typeof(Rank)))
            {
                deck.Add(new Card(suit, rank));
            }
        }

        return deck;
    }

    public void Draw(IDisplay display, int x, int y, bool isPlayer1, IFont font)
    {
        var color = GetSuitColor(Suit);
        display.DrawRectangle(x, y, CardWidth, CardHeight, color, Color.White);

        if (isPlayer1)
        {
            font.DrawText(display, x + 2, y + 2, color, Rank.ToString());
            font.DrawText(display, x + CardWidth - 8, y + CardHeight - 12, color, Suit.ToString());
        }
        else
        {
            font.DrawText(display, x + CardWidth - 8, y + 2, color, Rank.ToString());
            font.DrawText(display, x + 2, y + CardHeight - 12, color, Suit.ToString());
        }
    }

    private Color GetSuitColor(Suit suit)
    {
        return (suit == Suit.Hearts || suit == Suit.Diamonds) ? Color.Red : Color.Black;
    }
}