using System.Drawing;
using System.Text.Json;
using Core.Display;
using Core.Fonts;
using Core.Inputs;
using Core.State;

namespace Core.Runner.RunnerElements;

public class Leaderboard : IPlayableGameElement
{
    private const int MaxEntries = 10;
    private readonly string gameName;
    private readonly string scoresFilePath;
    private List<(string, int)> highScores = new();
    private bool isNewHighScore;
    private bool showPlayAgainPrompt;

    private int _initialsIndex;
    private string _initials;
    private int _score;

    private readonly IFont font;
    private readonly Color textColor = Color.Green;
    private readonly Color highlightedTextColor = Color.Red;

    public Leaderboard(string gameName, IFontFactory fontFactory)
    {
        this.gameName = gameName;
        scoresFilePath = $"{gameName}.json";
        font = fontFactory.GetFont(LedFontType.Font5x7);
        LoadScoresFromFile();
    }

    private void LoadScoresFromFile()
    {
        try
        {
            var scoresJson = File.ReadAllText(scoresFilePath);
            highScores = JsonSerializer.Deserialize<List<(string, int)>>(scoresJson)!;
        }
        catch (Exception)
        {
            highScores = new List<(string, int)>();
        }
    }

    private void SaveScoresToFile()
    {
        var scoresJson = JsonSerializer.Serialize(highScores);
        File.WriteAllText(scoresFilePath, scoresJson);
    }

    private void AddScoreToLeaderboard()
    {
        highScores.Add((_initials, _score));
        highScores = highScores.OrderByDescending(s => s.Item2).Take(MaxEntries).ToList();
        SaveScoresToFile();
    }

    public void HandleInput(IPlayerConsole player1Console)
    {
        if (showPlayAgainPrompt)
        {
            var buttons = player1Console.ReadButtons();
            if (buttons == Buttons.Green)
            {
                // restart game
                State = GameOverState.None;
                showPlayAgainPrompt = false;
                isNewHighScore = false;
            }
            else if (buttons == Buttons.Red)
            {
                // quit game
                State = GameOverState.EndOfGame;
            }

            return;
        }

        if (isNewHighScore)
        {
            HandleInitialsInput(player1Console);
            return;
        }

        // any button press to go back to main menu
        if (player1Console.ReadButtons() != Buttons.None)
        {
            State = GameOverState.None;
        }
    }

    private void HandleInitialsInput(IPlayerConsole player1Console)
    {
        var joystick = player1Console.ReadJoystick();
        var buttons = player1Console.ReadButtons();

        if (joystick == JoystickDirection.Right && _initialsIndex < 2)
        {
            _initialsIndex++;
        }
        else if (joystick == JoystickDirection.Left && _initialsIndex > 0)
        {
            _initialsIndex--;
        }
        else if (joystick == JoystickDirection.Up)
        {
            var newChar = (char)(((int)_initials[_initialsIndex] + 1 - 65) % 26 + 65);
            _initials = _initials[.._initialsIndex] + newChar + _initials[(_initialsIndex + 1)..];
        }
        else if (joystick == JoystickDirection.Down)
        {
            var newChar = (char)(((int)_initials[_initialsIndex] - 1 - 65 + 26) % 26 + 65);
            _initials = _initials[.._initialsIndex] + newChar + _initials[(_initialsIndex + 1)..];
        }

        if (buttons == Buttons.Green)
        {
            AddScoreToLeaderboard();
            isNewHighScore = false;
            showPlayAgainPrompt = true;
        }
    }

    public void Update()
    {
        // not used in this game element
    }

    public void Draw(IDisplay display)
    {
        // draw title
        font.DrawText(display, 4, 4, textColor, $"{gameName} High Scores");

        // draw scores
        for (var i = 0; i < highScores.Count; i++)
        {
            var (initials, score) = highScores[i];
            var color = i == 0 && isNewHighScore ? highlightedTextColor : this.textColor;
            var y = 16 + i * 8;
            font.DrawText(display, 4, y, color, $"{initials} {score}");
        }

        // prompt for entering initials
        if (isNewHighScore)
        {
            font.DrawText(display, 4, 54, textColor, "Enter your initials:");
            font.DrawText(display, 44, 54, textColor, _initials, vertical: false, spacing: 6);
            font.DrawText(display, 44 + _initialsIndex * 6, 60, highlightedTextColor, "^");
        }

        // prompt to play again
        if (showPlayAgainPrompt)
        {
            font.DrawText(display, 4, 54, textColor, "Play again?");
            font.DrawText(display, 4, 60, textColor, "(Green to play, Red to quit)");
        }
    }

    public GameOverState State { get; private set; }

    public void SetScore(int score)
    {
        this._score = score;
        isNewHighScore = highScores.Count < MaxEntries || this._score > highScores.Last().Item2;
        if (isNewHighScore)
        {
            _initials = "AAA";
            _initialsIndex = 0;
        }
    }
}