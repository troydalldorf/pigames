namespace MemoryCard.Bits;

public class Card
{
    private DateTime? autoUnselectAt;
    public CardShape Shape { get; }
    public int Row { get; set; }
    public int Column { get; set; }

    private CardState state;
    public CardState State
    {
        get
        {
            if (state == CardState.Unselecting && autoUnselectAt < DateTime.Now)
            {
                state = CardState.FaceDown;
            }

            return state;
        }
        set
        {
            state = value;
            if (state == CardState.Unselecting)
            {
                autoUnselectAt = DateTime.Now.AddSeconds(2);
            }
            else
            {
                autoUnselectAt = null;
            }
        }
    }

    public Card(CardShape shape, int row, int column)
    {
        Shape = shape;
        Row = row;
        Column = column;
        State = CardState.FaceDown;
    }
}