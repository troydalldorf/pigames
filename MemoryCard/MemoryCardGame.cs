using System.Diagnostics;
using System.Drawing;
using Core;
using MemoryCard.Bits;

namespace MemoryCard;

public class MemoryCardGame : IPlayableGameElement
{
    private const int Rows = 4;
    private const int Columns = 5;
    private const int CardSize = 8;
    private const int CardSpacing = 2;

    private Card[,] cards;
    private Card? firstSelectedCard;
    private Card? secondSelectedCard;
    private int cursorCol = 0;
    private int cursorRow = 0;
    private readonly Stopwatch stopwatch;
    private long lastActionAt;

    public MemoryCardGame()
    {
        this.stopwatch = new Stopwatch();
        this.stopwatch.Start();
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
        int totalCardShapes = Enum.GetValues(typeof(CardShape)).Length;
        int totalPairs = Rows * Columns / 2;

        for (var i = 0; i < totalPairs; i++)
        {
            var shape = (CardShape)(i % totalCardShapes);
            shapes.Add(shape);
            shapes.Add(shape);
        }

        return shapes;
    }

    public void HandleInput(IPlayerConsole playerConsole)
    {
        if (stopwatch.ElapsedMilliseconds - lastActionAt < 120)
            return;
        var stick = playerConsole.ReadJoystick();
        var buttons = playerConsole.ReadButtons();

        if (stick.IsUp())
        {
            cursorRow = Math.Max(cursorRow - 1, 0);
            lastActionAt = stopwatch.ElapsedMilliseconds;
        }

        if (stick.IsDown())
        {
            cursorRow = Math.Min(cursorRow + 1, Rows - 1);
            lastActionAt = stopwatch.ElapsedMilliseconds;
        }

        if (stick.IsLeft())
        {
            cursorCol = Math.Max(cursorCol - 1, 0);
            lastActionAt = stopwatch.ElapsedMilliseconds;
        }

        if (stick.IsRight())
        {
            cursorCol = Math.Min(cursorCol + 1, Columns - 1);
            lastActionAt = stopwatch.ElapsedMilliseconds;
        }

        var selectedCard = cards[cursorRow, cursorCol];

        if (buttons.IsGreenPushed())
        {
            lastActionAt = stopwatch.ElapsedMilliseconds;
            if (selectedCard.State.HasFlag(CardState.Matched))
                return;

            if (firstSelectedCard == null)
            {
                firstSelectedCard = selectedCard;
                firstSelectedCard.State = CardState.Selected;
            }
            else if (secondSelectedCard == null && firstSelectedCard != selectedCard) // Ensure the second card is different from the first one
            {
                secondSelectedCard = selectedCard;
                secondSelectedCard.State = CardState.Selected;
            }

            if (firstSelectedCard != null && secondSelectedCard != null)
            {
                if (firstSelectedCard.Shape == secondSelectedCard.Shape)
                {
                    firstSelectedCard.State = CardState.Matched;
                    secondSelectedCard.State = CardState.Matched;
                }

                firstSelectedCard.State = secondSelectedCard.State = CardState.Unselecting;
                firstSelectedCard = secondSelectedCard = null;
            }
        }

        if (IsAllMatched()) State = GameOverState.Player1Wins;
    }

    private bool IsAllMatched()
    {
        return cards.Cast<Card>().All(card => card.State == CardState.Matched);
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

                if (card.State == CardState.FaceDown)
                {
                    display.DrawRectangle(x, y, CardSize, CardSize, Color.Gray, Color.Gray);
                    display.SetPixel(x, y, Color.Black);
                    display.SetPixel(x + CardSize, y, Color.Black);
                    display.SetPixel(x + CardSize, y + CardSize, Color.Black);
                    display.SetPixel(x, y + CardSize, Color.Black);
                }
                else
                {
                    var color = card.State == CardState.Matched ? Color.Yellow : Color.Gray;
                    card.Shape.Draw(display, x, y, CardSize);
                }

                if (cursorCol == col && cursorRow == row)
                {
                    display.DrawRectangle(x, y, CardSize, CardSize, Color.LimeGreen);
                }
            }
        }
    }

    public GameOverState State { get; private set; }
}