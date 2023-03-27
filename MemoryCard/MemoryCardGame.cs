using System.Collections;
using System.Diagnostics;
using System.Drawing;
using Core;
using MemoryCard.Bits;

namespace MemoryCard;

public class MemoryCardGame : IPlayableGameElement
{
    private const int CardSize = 10;
    private const int CardSpacing = 2;

    private Level level;
    private Card[,] cards;
    private Card? firstSelectedCard;
    private Card? secondSelectedCard;
    private int cursorCol = 0;
    private int cursorRow = 0;
    private int unselectingCount = 0;
    private readonly Stopwatch stopwatch;
    private long lastActionAt;

    public MemoryCardGame()
    {
        this.stopwatch = new Stopwatch();
        this.stopwatch.Start();
        Initialize(1);
    }

    private void Initialize(int levelNo)
    { 
        level = LevelFactory.GetLevel(levelNo);
        State = GameOverState.None;
        cards = new Card[level.Rows, level.Columns];
        var cardShapes = GenerateCardShapes(level.Shapes);
        var random = new Random();

        for (var row = 0; row < level.Rows; row++)
        {
            for (var col = 0; col < level.Columns; col++)
            {
                if (cardShapes.Count == 0) // Odd number of cards, e.g. 5x5
                {
                    var card = new Card(CardShape.Blank, row, col);
                    card.State = CardState.Matched;
                    cards[row, col] = card;
                }
                else
                {
                    var index = random.Next(cardShapes.Count);
                    cards[row, col] = new Card(cardShapes[index], row, col);
                    cardShapes.RemoveAt(index);
                }
            }
        }
    }

    private List<CardShape> GenerateCardShapes(IList<CardShape> availableShapes)
    {
        var shapes = new List<CardShape>();
        var totalCardShapes = availableShapes.Count;
        var totalPairs = level.Rows * level.Columns / 2;

        for (var i = 0; i < totalPairs; i++)
        {
            var shape = availableShapes[i % totalCardShapes];
            shapes.Add(shape);
            shapes.Add(shape);
        }

        return shapes;
    }

    public void HandleInput(IPlayerConsole playerConsole)
    {
        if (stopwatch.ElapsedMilliseconds - lastActionAt < 200)
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
            cursorRow = Math.Min(cursorRow + 1, level.Rows - 1);
            lastActionAt = stopwatch.ElapsedMilliseconds;
        }

        if (stick.IsLeft())
        {
            cursorCol = Math.Max(cursorCol - 1, 0);
            lastActionAt = stopwatch.ElapsedMilliseconds;
        }

        if (stick.IsRight())
        {
            cursorCol = Math.Min(cursorCol + 1, level.Columns - 1);
            lastActionAt = stopwatch.ElapsedMilliseconds;
        }

        var selectedCard = cards[cursorRow, cursorCol];

        if (buttons.IsGreenPushed())
        {
            // prevent flipping cards too fast
            if (unselectingCount > 0)
            {
                cards.Cast<Card>()
                    .Where(x => x.State == CardState.Unselecting)
                    .ToList()
                    .ForEach(x => x.State = CardState.FaceDown);
                unselectingCount = 0;
            }
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
                else
                {
                    firstSelectedCard.State = secondSelectedCard.State = CardState.Unselecting;
                    unselectingCount = 2;
                }
                firstSelectedCard = secondSelectedCard = null;
            }
        }

        if (IsAllMatched())
        {
            if (level.LevelNo < LevelFactory.MaxLevel)
                Initialize(level.LevelNo + 1);
            else
            {
                State = GameOverState.Player1Wins;
            }
        }
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
        var xOffset = (64 - level.Columns * (CardSize + CardSpacing)) / 2;
        var yOffset = (64 - level.Rows * (CardSize + CardSpacing)) / 2;
        for (var row = 0; row < level.Rows; row++)
        {
            for (var col = 0; col < level.Columns; col++)
            {
                var card = cards[row, col];
                var x = xOffset + col * (CardSize + CardSpacing);
                var y = yOffset + row * (CardSize + CardSpacing);

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
                    var color = card.State switch
                    {
                        CardState.Selected => Color.Yellow,
                        CardState.Unselecting => Color.PaleGoldenrod,
                        _ => Color.LimeGreen
                    };
                    card.Shape.Draw(display, x, y, CardSize, color);
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