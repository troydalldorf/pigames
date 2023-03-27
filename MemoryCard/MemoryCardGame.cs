using System.Drawing;
using Core;

public class MemoryCardGame : IPlayableGameElement
{
    private const int Rows = 4;
    private const int Columns = 8;
    private const int CardSize = 12;
    private const int CardSpacing = 2;

    private Card[,] cards;
    private Card? firstSelectedCard;
    private Card? secondSelectedCard;
    private int cursorCol = 0;
    private int cursorRow = 0;

    public MemoryCardGame()
    {
        Initialize();
    }

    private void Initialize()
    {
        State = GameOverState.None;
        cards = new Card[Rows, Columns];
        var cardShapes = GenerateCardShapes();
        var random = new Random();

        for (var row = 0; row < Rows; row++)
        {
            for (var col = 0; col < Columns; col++)
            {
                var index = random.Next(cardShapes.Count);
                cards[row, col] = new Card(cardShapes[index], row, col);
                cardShapes.RemoveAt(index);
            }
        }
    }

    private static List<CardShape> GenerateCardShapes()
    {
        var shapes = new List<CardShape>();
        for (var i = 0; i < Rows * Columns / 2; i++)
        {
            var shape = (CardShape)i;
            shapes.Add(shape);
            shapes.Add(shape);
        }
        return shapes;
    }

    public void HandleInput(IPlayerConsole playerConsole)
    {
        var stick = playerConsole.ReadJoystick();
        var buttons = playerConsole.ReadButtons();

        if (stick.IsUp()) cursorRow = Math.Max(cursorRow - 1, 0);
        if (stick.IsDown()) cursorRow = Math.Min(cursorRow + 1, Rows - 1);
        if (stick.IsLeft()) cursorCol = Math.Max(cursorCol - 1, 0);
        if (stick.IsRight()) cursorCol = Math.Min(cursorCol + 1, Columns - 1);
        var selectedCard = cards[cursorCol, cursorRow];

        if (buttons.HasFlag(Buttons.Green))
        {
            if (!selectedCard.IsMatched)
            {
                if (firstSelectedCard == null)
                {
                    firstSelectedCard = selectedCard;
                    firstSelectedCard.IsSelected = true;
                }
                else if (secondSelectedCard == null)
                {
                    secondSelectedCard = selectedCard;
                    secondSelectedCard.IsSelected = true;
                }

                if (firstSelectedCard != null && secondSelectedCard != null)
                {
                    if (firstSelectedCard?.Shape == secondSelectedCard?.Shape)
                    {
                        firstSelectedCard.IsMatched = true;
                        secondSelectedCard.IsMatched = true;
                    }
                    else
                    {
                        firstSelectedCard.IsSelected = false;
                        secondSelectedCard.IsSelected = false;
                    }                    
                }
            }
        }

        if (IsAllMatched()) State = GameOverState.Player1Wins;
    }

    private bool IsAllMatched()
    {
        return cards.Cast<Card?>().All(card => card.IsMatched);
    }

    public void Update()
    {
        // No update logic needed for this game.
    }

    public void Draw(IDisplay display)
    {
        for (var row = 0; row < Rows; row++)
        {
            for (var col = 0; col < Columns; col++)
            {
                var card = cards[row, col];
                var x = col * (CardSize + CardSpacing);
                var y = row * (CardSize + CardSpacing);

                if (!card.IsSelected && !card.IsMatched)
                {
                    display.DrawRectangle(x, y, CardSize, CardSize, Color.Gray, Color.Gray);
                }
                else
                {
                    card.Shape.Draw(display, x, y, CardSize);
                }
                if (cursorCol == col && cursorRow == row)
                {
                    display.DrawRectangle(x, y, CardSize, CardSize, Color.Red);
                }
            }
        }
    }

    public GameOverState State { get; private set; }
}