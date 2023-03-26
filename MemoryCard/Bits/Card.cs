public class Card
{
    public CardShape Shape { get; }
    public int Row { get; set; }
    public int Column { get; set; }
    public bool IsSelected { get; set; }
    public bool IsMatched { get; set; }

    public Card(CardShape shape, int row, int column)
    {
        Shape = shape;
        Row = row;
        Column = column;
        IsSelected = false;
        IsMatched = false;
    }
}