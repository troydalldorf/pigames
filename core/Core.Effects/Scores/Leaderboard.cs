using System.Drawing;
using System.Text.Json;
using Core;
using Core.Display.Fonts;

public class Leaderboard : IPlayableGameElement
{
    private const int MaxEntries = 10;
    private readonly string gameName;
    private readonly string scoresFilePath;
    private List<(string, int)>? highScores;
    private bool isNewHighScore;
    private bool showPlayAgainPrompt;

    private string initials;
    private int score;
    private int initialsIndex;

    private readonly LedFont font;
    private readonly Color textColor = Color.Green;
    private readonly Color highlightedTextColor = Color.Red;
    private readonly Color backgroundColor = Color.Black;

    public Leaderboard(string gameName)
    {
        this.gameName = gameName;
        scoresFilePath = $"{gameName}.json";

        font = new LedFont(LedFontType.Font5x7);

        LoadScoresFromFile();
    }

    private void LoadScoresFromFile()
    {
        try
        {
            var scoresJson = File.ReadAllText(scoresFilePath);
            highScores = JsonSerializer.Deserialize<List<(string, int)>>(scoresJson);
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
        highScores.Add((initials, score));
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
            return;
        }
    }

    private void HandleInitialsInput(IPlayerConsole player1Console)
    {
        var joystick = player1Console.ReadJoystick();
        var buttons = player1Console.ReadButtons();

        if (joystick == JoystickDirection.Right && initialsIndex < 2)
        {
            initialsIndex++;
        }
        else if (joystick == JoystickDirection.Left && initialsIndex > 0)
        {
            initialsIndex--;
        }
        else if (joystick == JoystickDirection.Up)
        {
            var newChar = (char)(((int)initials[initialsIndex] + 1 - 65) % 26 + 65);
            initials = initials[..initialsIndex] + newChar + initials[(initialsIndex + 1)..];
        }
        else if (joystick == JoystickDirection.Down)
        {
            var newChar = (char)(((int)initials[initialsIndex] - 1 - 65 + 26) % 26 + 65);
            initials = initials[..initialsIndex] + newChar + initials[(initialsIndex + 1)..];
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
        for (int i = 0; i < highScores.Count; i++)
        {
            var (initials, score) = highScores[i];
            var textColor = i == 0 && isNewHighScore ? highlightedTextColor : this.textColor;
            var y = 16 + i * 8;
            font.DrawText(display, 4, y, textColor, $"{initials} {score}");
        }

        // prompt for entering initials
        if (isNewHighScore)
        {
            font.DrawText(display, 4, 54, textColor, "Enter your initials:");
            font.DrawText(display, 44, 54, textColor, initials, vertical: false, spacing: 6);
            font.DrawText(display, 44 + initialsIndex * 6, 60, highlightedTextColor, "^");
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
        this.score = score;
        isNewHighScore = highScores.Count < MaxEntries || this.score > highScores.Last().Item2;
        if (isNewHighScore)
        {
            initials = "AAA";
            initialsIndex = 0;
        }
    }
}