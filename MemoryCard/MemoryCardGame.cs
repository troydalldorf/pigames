using System.Drawing;
using Core;

public class MemoryCardGame : IPlayableGameElement
{
    private const int Rows = 4;
    private const int Columns = 8;
    private const int CardSize = 12;
    private const int CardSpacing = 2;

    private Card[,] cards;
    private Card firstSelectedCard;
    private Card secondSelectedCard;
    private int cursorX = 0;
    private int cursorY = 0;

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
        firstSelectedCard = cards[0, 0];
        firstSelectedCard.IsSelected = false;
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

        if (stick.IsUp()) cursorY = Math.Max(cursorY - 1, 0);
        if (stick.IsDown()) cursorY = Math.Min(cursorY + 1, Rows - 1);
        if (stick.IsLeft()) cursorX = Math.Max(cursorX - 1, 0);
        if (stick.IsRight()) cursorX = Math.Min(cursorX + 1, Columns - 1);
        var selectedCard = cards[cursorX, cursorY];

        if (buttons.HasFlag(Buttons.Green) && firstSelectedCard != null && secondSelectedCard == null)
        {
            if (!selectedCard.IsMatched)
            {
                if (secondSelectedCard == null)
                {
                    secondSelectedCard = selectedCard;
                    secondSelectedCard.IsSelected = true;
                }

                if (firstSelectedCard.Shape == secondSelectedCard.Shape)
                {
                    firstSelectedCard.IsMatched = true;
                    secondSelectedCard.IsMatched = true;
                }
                else
                {
                    firstSelectedCard.IsSelected = false;
                    secondSelectedCard.IsSelected = false;
                }

                firstSelectedCard = secondSelectedCard;
                secondSelectedCard = null;
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
                if (cursorX == x && cursorY == y)
                {
                    display.DrawRectangle(x, y, CardSize, CardSize, Color.Red);
                }
            }
        }
    }

    public GameOverState State { get; private set; }
}

public enum CardShape
{
    Circle,
    Triangle,
    Square,
    Diamond,
    Star,
    Plus,
    Cross,
    Hexagon
}

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

public static class CardShapeExtensions
{
    public static void Draw(this CardShape shape, IDisplay display, int x, int y, int size)
    {
        var color = Color.FromArgb(255, 255, 255);

        switch (shape)
        {
            case CardShape.Circle:
                display.DrawCircle(x + size / 2, y + size / 2, size / 2, color);
                break;
            case CardShape.Triangle:
                display.DrawLine(x, y + size, x + size / 2, y, color);
                display.DrawLine(x + size / 2, y, x + size, y + size, color);
                display.DrawLine(x + size, y + size, x, y + size, color);
                break;
            case CardShape.Square:
                display.DrawRectangle(x, y, size, size, color, color);
                break;
            case CardShape.Diamond:
                display.DrawLine(x + size / 2, y, x, y + size / 2, color);
                display.DrawLine(x, y + size / 2, x + size / 2, y + size, color);
                display.DrawLine(x + size / 2, y + size, x + size, y + size / 2, color);
                display.DrawLine(x + size, y + size / 2, x + size / 2, y, color);
                break;
            case CardShape.Star:
                // Add star drawing logic here.
                break;
            case CardShape.Plus:
                var lineWidth = size / 4;
                display.DrawRectangle(x + lineWidth, y, lineWidth, size, color, color);
                display.DrawRectangle(x, y + lineWidth, size, lineWidth, color, color);
                break;
            case CardShape.Cross:
                display.DrawLine(x, y, x + size, y + size, color);
                display.DrawLine(x + size, y, x, y + size, color);
                break;
            case CardShape.Hexagon:
                var xOffset = size / 4;
                var yOffset = size / 2;
                display.DrawLine(x + xOffset, y, x + size - xOffset, y, color);
                display.DrawLine(x, y + yOffset / 2, x + xOffset, y, color);
                display.DrawLine(x, y + yOffset * 3 / 2, x + xOffset, y + yOffset * 2, color);
                display.DrawLine(x + size, y + yOffset / 2, x + size - xOffset, y, color);
                display.DrawLine(x + size, y + yOffset * 3 / 2, x + size - xOffset, y + yOffset * 2, color);
                display.DrawLine(x + xOffset, y + yOffset * 2, x + size - xOffset, y + yOffset * 2, color);
                break;
        }
    }
}
