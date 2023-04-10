using Core;
using Core.Fonts;
using System.Drawing;
using MastermindGame.Bits;

namespace MastermindGame;

public class MastermindGame : IPlayableGameElement
{
    private const int CodeLength = 4;
    private const int MaxAttempts = 10;
    private const int CellSize = 4;
    private const int Spacing = 2;

    private Code secretCode;
    private List<Guess> playerGuesses;
    private readonly IFont font;
    private int cursorPosition = 0;

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
        var h1 = CellSize + Spacing * 2 + 2;
        var w = CellSize + Spacing * 2 + 2;
        display.DrawRectangle(0, 0, (CellSize + Spacing * 2) * CodeLength + Spacing,h1, Color.Gray, Color.Gray);
        var h2 = (CellSize + Spacing) * MaxAttempts + Spacing + 2;
        display.DrawRectangle(0, h1+1, (CellSize + Spacing) * CodeLength + Spacing + 2, h2, Color.Gray);
        var xOffset = 1;
        var y = h1 + h2 + 1 - CellSize - Spacing;
        if (State != GameOverState.None)
        {
            for (var i = 0; i < CodeLength; i++)
            {
                if (secretCode[i] == null) continue;
                display.DrawRectangle(xOffset + i * (CellSize + Spacing), h1+2+Spacing, CellSize, CellSize, Colors[(int)secretCode[i]!.Value]);
            }
        }

        // Draw the player's guesses
        for (var attempt = 0; attempt < playerGuesses.Count; attempt++)
        {
            var guess = playerGuesses[attempt];

            for (var i = 0; i < CodeLength; i++)
            {
                if (secretCode[i] == null) continue;
                display.DrawRectangle(xOffset + i * CellSize * 2 + Spacing, y, CellSize, CellSize, Color.Black, Colors[(int)guess[i]!.Value]);
                y -= CellSize + Spacing*2;
            }

            var guessX = w;
            for (var i = 0; i < guess.CorrectColorAndPosition; i++)
            {
                display.SetPixel(guessX++, y + CellSize/2, Color.White);
            }
            for (var i = 0; i < guess.CorrectColorOnly; i++)
            {
                display.SetPixel(guessX++, y + CellSize/2, Color.Red);
            }
        }
    }

    public GameOverState State { get; private set; } = GameOverState.None;
}