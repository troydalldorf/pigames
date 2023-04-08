namespace Speed.Bits;

public class Player
{
    private Queue<Card> cards;
    
    public Player(IEnumerable<Card> cards)
    {
        this.cards = new Queue<Card>(cards);
        this.CurrentCard = null;
        this.Score = 0;
    }
    
    public bool IsTurn { get; set; }
    
    public Card? CurrentCard { get; private set; }
    
    public int Score { get; set; }
    
    public bool HasCards => this.cards.Count > 0;

    public void NextCard()
    {
        this.CurrentCard = cards.Dequeue();
    }

    public void ScoreMatch() => this.Score++;
    
    public void ScoreMiss() => this.Score = Math.Max(0, this.Score-1);
}