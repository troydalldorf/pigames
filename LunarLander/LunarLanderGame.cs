using System.Drawing;
using Core;
using Core.Fonts;
using LunarLander.Bits;

namespace LunarLander;

public class LunarLanderGame : IDuoPlayableGameElement
{
    private readonly IFont font;
    private readonly Lander lander;
    private readonly Terrain terrain;
    private readonly LandingPad landingPad;
    private GameOverState state;

    public LunarLanderGame(IFontFactory fontFactory)
    {
        font = fontFactory.GetFont(LedFontType.Font4x6);
        lander = new Lander();
        terrain = new Terrain();
        landingPad = new LandingPad();
        state = GameOverState.None;
    }

    public void HandleInput(IPlayerConsole player1Console)
    {
        // Check joystick and move lander
        var direction = player1Console.ReadJoystick();
        switch (direction)
        {
            case JoystickDirection.Up:
                lander.MoveUp();
                break;
            case JoystickDirection.Down:
                lander.MoveDown();
                break;
            case JoystickDirection.Left:
                lander.MoveLeft();
                break;
            case JoystickDirection.Right:
                lander.MoveRight();
                break;
            default:
                lander.Stabilize();
                break;
        }
    }

    public void Handle2PInput(IPlayerConsole player2Console)
    {
        // For now, we'll ignore the second player input
    }

    public void Update()
    {
        // Check for collisions and update the game state
        if (lander.IsCollidingWith(terrain) || lander.IsCollidingWith(landingPad))
        {
            state = GameOverState.EndOfGame;
        }
        else if (lander.HasLandedOn(landingPad))
        {
            state = GameOverState.Player1Wins;
        }

        lander.Update();
    }

    public void Draw(IDisplay display)
    {
        // Draw lander
        lander.Draw(display);

        // Draw terrain and landing pad
        terrain.Draw(display);
        landingPad.Draw(display);

        // Draw game over or win message if game is over
        if (state == GameOverState.EndOfGame)
        {
            font.DrawText(display, display.Width / 2, display.Height / 2, Color.Red, "Game Over");
        }
        else if (state == GameOverState.Player1Wins)
        {
            font.DrawText(display, display.Width / 2, display.Height / 2, Color.Green, "You Win!");
        }
    }

    public GameOverState State => state;
}