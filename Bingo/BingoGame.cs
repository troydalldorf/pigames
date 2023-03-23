using System.Drawing;
using Core;
using Core.Display.Fonts;

public class BingoGame : IGameElement
{
    private LedFont font;
    private int[,] player1Card;
    private int[,] player2Card;
    private List<int> numbers = new();
    private bool isDone;
    private Random random = new();

    public BingoGame()
    {
        font = new LedFont(LedFontType.FontTomThumb);

        // Generate random numbers for the game
        for (var i = 1; i <= 25; i++)
        {
            numbers.Add(i);
        }

        Shuffle(numbers);

        // Generate bingo cards for each player
        player1Card = GenerateCard();
        player2Card = GenerateCard();
    }

    public void HandleInput(IPlayerConsole playerConsole)
    {
        Buttons buttons = playerConsole.ReadButtons();

        if ((buttons & Buttons.Green) == Buttons.Green)
        {
            // Draw a number and check if it exists on a player's card
            int number = numbers[0];
            numbers.RemoveAt(0);

            if (CheckCard(player1Card, number) || CheckCard(player2Card, number))
            {
                // If the number is on a player's card, mark it off
                if (CheckCard(player1Card, number))
                {
                    MarkCard(player1Card, number);
                }

                if (CheckCard(player2Card, number))
                {
                    MarkCard(player2Card, number);
                }

                // Check for a winner
                if (CheckWinner(player1Card) || CheckWinner(player2Card))
                {
                    isDone = true;
                }
            }
        }
    }

    public void Update()
    {
        // No additional updates needed
    }

    public void Draw(IDisplay display)
    {
        DrawCard(display, player1Card, 2, 2, currentPlayer: 1);
        DrawCard(display, player2Card, 34, 2, currentPlayer: 2);
    }

    public bool IsDone() => isDone;

    private int[,] GenerateCard()
    {
        // Generate a random bingo card for a player
        var card = new int[5, 5];

        for (var col = 0; col < 5; col++)
        {
            var columnNumbers = new List<int>();
            var minNumber = col * 15 + 1;
            var maxNumber = minNumber + 14;

            for (var row = 0; row < 5; row++)
            {
                int number;

                do
                {
                    number = random.Next(minNumber, maxNumber + 1);
                } while (columnNumbers.Contains(number));

                card[row, col] = number;
                columnNumbers.Add(number);
            }
        }

        // Mark the center square as already checked
        card[2, 2] = -1;

        return card;
    }

    private bool CheckCard(int[,] card, int number)
    {
        // Check if a number exists on a player's card
        for (int row = 0; row < 5; row++)
        {
            for (int col = 0; col < 5; col++)
            {
                if (card[row, col] == number)
                {
                    return true;
                }
            }
        }

        return false;
    }

    private void MarkCard(int[,] card, int number)
    {
        {
            // Mark a number as checked on a player's card
            for (int row = 0; row < 5; row++)
            {
                for (int col = 0; col < 5; col++)
                {
                    if (card[row, col] == number)
                    {
                        card[row, col] = -1;
                    }
                }
            }
        }
    }

    private bool CheckWinner(int[,] card)
    {
        // Check if a player has marked all numbers in a row, column, or diagonal
        for (var i = 0; i < 5; i++)
        {
            // Check row
            if (card[i, 0] == -1 && card[i, 1] == -1 && card[i, 2] == -1 && card[i, 3] == -1 && card[i, 4] == -1)
            {
                return true;
            }

            // Check column
            if (card[0, i] == -1 && card[1, i] == -1 && card[2, i] == -1 && card[3, i] == -1 && card[4, i] == -1)
            {
                return true;
            }
        }

        // Check diagonals
        if (card[0, 0] == -1 && card[1, 1] == -1 && card[2, 2] == -1 && card[3, 3] == -1 && card[4, 4] == -1)
        {
            return true;
        }

        if (card[0, 4] == -1 && card[1, 3] == -1 && card[2, 2] == -1 && card[3, 1] == -1 && card[4, 0] == -1)
        {
            return true;
        }

        return false;
    }

    private void DrawCard(IDisplay display, int[,] card, int x, int y, int currentPlayer)
    {
        var rowHeight = 6;
        var colWidth = 12;
        var yOffset = currentPlayer == 1 ? 32 : 0;
        // Draw a player's bingo card
        for (var row = 0; row < 5; row++)
        {
            for (var col = 0; col < 5; col++)
            {
                var number = card[row, col];
                if (number == -1)
                {
                    display.DrawRectangle(x + col * colWidth, yOffset + y + rowHeight * 12, colWidth, colWidth, currentPlayer == 1 ? Color.Green : Color.Yellow, currentPlayer == 1 ? Color.Black : Color.Black);
                }
                else
                {
                    font.DrawText(display, x + col * colWidth, yOffset + y + rowHeight * 12, currentPlayer == 1 ? Color.Green : Color.Yellow, number.ToString());
                }
            }
        }
    }

    private void Shuffle<T>(List<T> list)
    {
        var n = list.Count;

        while (n > 1)
        {
            n--;
            var k = random.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}