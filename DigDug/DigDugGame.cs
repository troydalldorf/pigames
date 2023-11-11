using System.Drawing;
using Core;
using Core.Display;
using Core.Fonts;
using Core.Inputs;
using Core.State;
using DigDug.Bits;

public class DigDugGame : IDuoPlayableGameElement
{
    private readonly Player player;
    private readonly List<Enemy> enemies;
    private readonly Tunnel tunnel;
    private readonly int score;
    private readonly IFont font;

    public DigDugGame(IFontFactory fontFactory)
    {
        font = fontFactory.GetFont(LedFontType.Font4x6);
        player = new Player(32, 32);
        enemies = new List<Enemy>
        {
            new Enemy(16, 16),
            new Enemy(48, 16)
        };
        tunnel = new Tunnel();
        score = 0;
        State = GameOverState.None;
    }

    public void HandleInput(IPlayerConsole player1Console)
    {
        // Handle player input for movement and attacking
        var stick = player1Console.ReadJoystick();
        player.HandleInput(stick);

        var buttons = player1Console.ReadButtons();
        if (buttons.HasFlag(Buttons.Green))
        {
            // Implement attack logic, e.g., inflating enemies or firing projectiles
        }
    }

    public void Handle2PInput(IPlayerConsole player2Console)
    {
        // Implement player 2 input if desired, following the same structure as HandleInput
    }

    public void Update()
    {
        // Update game elements
        player.Update();
        tunnel.Update(player);
        foreach (var enemy in enemies)
        {
            enemy.Update();
        }

        // Implement collision detection, scoring, and game over conditions
    }

    public void Draw(IDisplay display)
    {
        display.Clear();

        // Draw game elements
        player.Draw(display);
        tunnel.Draw(display);
        foreach (var enemy in enemies)
        {
            enemy.Draw(display);
        }

        // Draw score
        font.DrawText(display, 0, 0, Color.White, $"Score: {score}");
    }

    public GameOverState State { get; private set; }
}