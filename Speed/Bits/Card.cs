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

    public void Draw(IDisplay display, int x, int y, Color? selectColor, IFont font)
    {
        var color = GetSuitColor(Suit);
        display.DrawRectangle(x, y, CardWidth, CardHeight, color, Color.White);
        font.DrawText(display, x + 2, y + 10, color, GetRankSymbol(Rank));
        font.DrawText(display, x + 8, y + 10, color, GetSuitSymbol(this.Suit));
        if (selectColor != null)
            display.DrawRectangle(x, y, CardWidth, CardHeight, selectColor.Value);
    }
    
    private static string GetRankSymbol(Rank rank)
    {
        return rank switch
        {
            Rank.N2 => "2",
            Rank.N3 => "3",
            Rank.N4 => "4",
            Rank.N5 => "5",
            Rank.N6 => "6",
            Rank.N7 => "7",
            Rank.N8 => "8",
            Rank.N9 => "9",
            Rank.T => "10",
            _ => rank.ToString()
        };
    }

    private static string GetSuitSymbol(Suit suit)
    {
        return suit switch
        {
            Suit.Clubs => "♣",
            Suit.Diamonds => "♦",
            Suit.Hearts => "♥",
            Suit.Spades => "♠",
            _ => throw new ArgumentOutOfRangeException(nameof(suit), suit, null)
        };
    }

    private Color GetSuitColor(Suit suit)
    {
        return suit is Suit.Hearts or Suit.Diamonds ? Color.Red : Color.Black;
    }
}