using System.Drawing;
using Core;
using Core.Display.Fonts;

public class BingoGame : IGameElement
{
    private LedFont font;
    private List<int> numbers = new List<int>();
    private List<int> player1Numbers = new List<int>();
    private List<int> player2Numbers = new List<int>();
    private int currentPlayer = 1;
    private bool isDone = false;

    public BingoGame()
    {
        font = new LedFont(LedFontType.Font5x7);

        for (var i = 1; i <= 25; i++)
        {
            numbers.Add(i);
        }

        Shuffle(numbers);
    }

    public void HandleInput(IPlayerConsole playerConsole)
    {
        var joystickDirection = playerConsole.ReadJoystick();
        var buttons = playerConsole.ReadButtons();

        // Switch player
        if ((buttons & Buttons.Blue) == Buttons.Blue)
        {
            currentPlayer = currentPlayer == 1 ? 2 : 1;
        }

        // Mark number if button is pressed
        if ((buttons & Buttons.Green) == Buttons.Green)
        {
            List<int> playerNumbers = currentPlayer == 1 ? player1Numbers : player2Numbers;

            if (playerNumbers.Count < 5)
            {
                playerNumbers.Add(numbers[0]);
                numbers.RemoveAt(0);
            }
        }
    }

    public void Update()
    {
        if (player1Numbers.Count == 5 || player2Numbers.Count == 5)
        {
            isDone = true;
        }
    }

    public void Draw(IDisplay display)
    {
        // Draw numbers on left side for player 1
        for (int i = 0; i < player1Numbers.Count; i++)
        {
            font.DrawText(display, 2, i * 8, Color.Green, player1Numbers[i].ToString(), vertical: true);
        }

        // Draw numbers on right side for player 2
        for (int i = 0; i < player2Numbers.Count; i++)
        {
            font.DrawText(display, 58, i * 8, Color.Yellow, player2Numbers[i].ToString(), vertical: true);
        }

        // Draw current player indicator
        if (currentPlayer == 1)
        {
            display.DrawRectangle(0, 32, 64, 32, Color.Black, Color.Green);
        }
        else
        {
            display.DrawRectangle(0, 32, 64, 32, Color.Black, Color.Yellow);
        }
    }

    public bool IsDone() => isDone;

    private void Shuffle<T>(List<T> list)
    {
        var random = new Random();
        var n = list.Count;

        while (n > 1)
        {
            n--;
            int k = random.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}
