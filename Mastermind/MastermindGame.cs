using Core;
using Core.Fonts;
using System.Drawing;

namespace MastermindGame;

public class MastermindGame : IPlayableGameElement
{
    private const int CodeLength = 4;
    private const int MaxAttempts = 10;
    private const int CellSize = 2;
    private const int Spacing = 1;

    private int[] secretCode;
    private List<int[]> playerGuesses;
    private List<(int, int)> guessResults; // (correct color and position, correct color only)
    private int currentAttempt;
    private readonly IFont font;

    private static readonly Color[] Colors = new[]
    {
        Color.White,
        Color.Yellow,
        Color.Orange,
        Color.Pink,
        Color.Green,
        Color.Purple
    };

    public MastermindGame(IFontFactory fontFactory)
    {
        this.font = fontFactory.GetFont(LedFontType.Font4x6);
        Initialize();
    }

    private void Initialize()
    {
        secretCode = GenerateSecretCode();
        currentAttempt = MaxAttempts - 1;
        playerGuesses = new List<int[]>(new int[MaxAttempts][]);
        playerGuesses[currentAttempt] = new int[CodeLength];
        guessResults = new List<(int, int)>();
    }

    private int[] GenerateSecretCode()
    {
        var random = new Random();
        int[] code = new int[CodeLength];
        for (int i = 0; i < CodeLength; i++)
        {
            code[i] = random.Next(Colors.Length);
        }

        return code;
    }

    public void HandleInput(IPlayerConsole playerConsole)
    {
        if (State != GameOverState.None) return;

        var direction = playerConsole.ReadJoystick();
        var buttonPressed = playerConsole.ReadButtons();

        if (direction != JoystickDirection.None)
        {
            var newGuess = new int[CodeLength];
            Array.Copy(playerGuesses[currentAttempt], newGuess, CodeLength);

            for (var i = 0; i < CodeLength; i++)
            {
                if (direction == JoystickDirection.Up)
                {
                    newGuess[i] = (newGuess[i] + 1) % Colors.Length;
                }
                else if (direction == JoystickDirection.Down)
                {
                    newGuess[i] = (newGuess[i] - 1 + Colors.Length) % Colors.Length;
                }
            }

            playerGuesses[currentAttempt] = newGuess;
        }
        else if (buttonPressed == Buttons.Green)
        {
            var result = CheckGuess(playerGuesses[currentAttempt]);
            guessResults.Add(result);
            currentAttempt++;

            if (result.Item1 == CodeLength || currentAttempt >= MaxAttempts)
            {
                State = result.Item1 == CodeLength ? GameOverState.Player1Wins : GameOverState.EndOfGame;
            }
        }
        else if (buttonPressed == Buttons.Red)
        {
            Initialize();
        }
    }

    private (int, int) CheckGuess(int[] guess)
    {
        var correctColorAndPosition = 0;
        var correctColorOnly = 0;

        var secretCodeCopy = (int[])secretCode.Clone();
        var guessCopy = (int[])guess.Clone();

        for (var i = 0; i < CodeLength; i++)
        {
            if (secretCodeCopy[i] == guessCopy[i])
            {
                correctColorAndPosition++;
                secretCodeCopy[i] = -1;
                guessCopy[i] = -2;
            }
        }

        for (var i = 0; i < CodeLength; i++)
        {
            for (var j = 0; j < CodeLength; j++)
            {
                if (secretCodeCopy[i] == guessCopy[j])
                {
                    correctColorOnly++;
                    secretCodeCopy[i] = -1;
                    guessCopy[j] = -2;
                }
            }
        }

        return (correctColorAndPosition, correctColorOnly);
    }

    public void Update()
    {
        // No continuous updates required for this game
    }

    public void Draw(IDisplay display)
    {
        display.DrawRectangle(0, 0, (CellSize + Spacing) * CodeLength + Spacing + 2, (CellSize + Spacing) * MaxAttempts + 2, Color.Blue, Color.Blue);
        var xOffset = 1;
        var yOffset = 5;
        display.DrawRectangle(0, 0, (CellSize + Spacing) * CodeLength + Spacing + 2, (CellSize + Spacing * 2), Color.White, Color.White);
        if (State != GameOverState.None)
        {
            for (int i = 0; i < CodeLength; i++)
            {
                int x = i * CellSize * 2;
                int y = 0;
                display.DrawRectangle(xOffset+x, yOffset+y, CellSize, CellSize, Color.Black, Colors[secretCode[i]]);
            }
        }

        // Draw the player's guesses
        for (int attempt = 0; attempt < playerGuesses.Count; attempt++)
        {
            int[] guess = playerGuesses[attempt];
            if (guess == null) continue;

            for (int i = 0; i < CodeLength; i++)
            {
                int x = i * CellSize * 2;
                int y = (MaxAttempts - attempt) * (CellSize * 2 + Spacing);
                display.DrawRectangle(xOffset+x, yOffset+y, CellSize, CellSize, Color.Black, Colors[guess[i]]);
            }

            if (attempt < guessResults.Count)
            {
                (int correctColorAndPosition, int correctColorOnly) = guessResults[attempt];
                int x = CodeLength * CellSize * 2;
                int y = (MaxAttempts - attempt) * (CellSize * 2 + Spacing);
                string result = $"{correctColorAndPosition}/{correctColorOnly}";
                font.DrawText(display, xOffset+x, yOffset+y, Color.White, result);
            }
        }
    }

    public GameOverState State { get; private set; } = GameOverState.None;
}