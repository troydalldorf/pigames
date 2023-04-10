using Core;
using Core.Fonts;
using System.Drawing;
using MastermindGame.Bits;

namespace MastermindGame;

public class MastermindGame : IPlayableGameElement
{
    private const int CodeLength = 4;
    private const int MaxAttempts = 10;
    private const int CellSize = 3;
    private const int Spacing = 2;

    private Code secretCode;
    private List<Guess> playerGuesses;
    private readonly IFont font;
    private int cursorPosition = 0;

    public MastermindGame(IFontFactory fontFactory)
    {
        this.font = fontFactory.GetFont(LedFontType.Font4x6);
        Initialize();
    }

    private void Initialize()
    {
        secretCode = Code.GenerateRandomCode();
        playerGuesses = new List<Guess>() { new Guess() };
    }

    public void HandleInput(IPlayerConsole playerConsole)
    {
        if (State != GameOverState.None) return;

        var direction = playerConsole.ReadJoystick();
        var buttonPressed = playerConsole.ReadButtons();

        if (direction == JoystickDirection.Left)
        {
            cursorPosition = (cursorPosition - 1 + CodeLength) % CodeLength;
        }
        else if (direction == JoystickDirection.Right)
        {
            cursorPosition = (cursorPosition + 1) % CodeLength;
        }
        else if (direction == JoystickDirection.Up)
        {
            playerGuesses.Last().NextValue(cursorPosition);
        }
        else if (direction == JoystickDirection.Down)
        {
            playerGuesses.Last().PrevValue(cursorPosition);
        }
        else if (buttonPressed == Buttons.Green)
        {
            playerGuesses.Last().CheckGuess(secretCode);
            if (playerGuesses.Last().CorrectColorAndPosition == CodeLength)
            {
                State = GameOverState.Player1Wins;
                return;
            }
            else if (playerGuesses.Count == MaxAttempts)
            {
                State = GameOverState.EndOfGame;
                return;
            }

            playerGuesses.Add(new Guess());
        }
        else if (buttonPressed == Buttons.Red)
        {
            Initialize();
        }
    }

    public void Update()
    {
        // No continuous updates required for this game
    }

    public void Draw(IDisplay display)
    {
        const int spacing = 2;
        const int xOffset = spacing;
        int y;

        // Draw the secret code area border
        display.DrawRectangle(0, 0, (CellSize + spacing) * CodeLength + spacing, CellSize + 2 * spacing, Color.Gray, Color.Gray);

        // Draw the board border
        display.DrawRectangle(0, CellSize + 2 * spacing, (CellSize + spacing) * CodeLength + spacing, (CellSize + spacing) * MaxAttempts + spacing, Color.Gray, Color.Gray);

        // Draw the secret code (only when the game is over)
        if (State != GameOverState.None)
        {
            y = spacing;
            for (var i = 0; i < CodeLength; i++)
            {
                display.DrawRectangle(xOffset + i * (CellSize + spacing), y, CellSize, CellSize, Color.Black, secretCode[i].ToColor());
            }
        }

        // Draw the player's guesses
        for (var attempt = 0; attempt < playerGuesses.Count; attempt++)
        {
            var guess = playerGuesses[attempt];
            y = (MaxAttempts - attempt) * (CellSize + spacing) + spacing + CellSize + 2 * spacing;
            if (attempt == playerGuesses.Count - 1)
            {
                // Draw the cursor
                display.DrawRectangle(xOffset + cursorPosition * (CellSize + spacing) - 1, y - 1, CellSize + 2, CellSize + 2, Color.White, Color.Transparent);
            }
            for (var i = 0; i < CodeLength; i++)
            {
                display.DrawRectangle(xOffset + i * (CellSize + spacing), y, CellSize, CellSize, guess[i].ToColor(), guess[i].ToColor());
            }

            // Draw the result for each guess
            var x = (CodeLength + 1) * (CellSize + spacing);
            var yResult = y + CellSize / 2;
            for (var i = 0; i < guess.CorrectColorAndPosition; i++)
            {
                display.SetPixel(x++, yResult, Color.White);
            }
            for (var i = 0; i < guess.CorrectColorOnly; i++)
            {
                display.SetPixel(x++, yResult, Color.Red);
            }
        }
    }

    public GameOverState State { get; private set; } = GameOverState.None;
}