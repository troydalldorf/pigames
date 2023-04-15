using System.Drawing;
using Core;
using Core.Display.Sprites;
using Core.Fonts;
using PacificWings.Bits;

namespace PacificWings;

public class PacificWingsGame : IPlayableGameElement
{
    private readonly Player player;
    private EnemyWave enemyWave;
    private SpriteAnimation enemySprite;
    private readonly IFont font;
    private int wave = 1;

    public PacificWingsGame(IFontFactory fontFactory)
    {
        var image = SpriteImage.FromResource("pwings.png");
        var playerSprite = image.GetSpriteAnimation(1, 1, 9, 8, 2, 1);
        this.enemySprite = image.GetSpriteAnimation(1, 15, 9, 8, 2, 1);
        var bulletSprite = image.GetSpriteAnimation(1, 10, 9, 3, 2, 1);
        player = new Player(32, 56, playerSprite, bulletSprite);
        enemyWave = EnemyWaveFactory.CreateWave(wave, enemySprite)!;
        font = fontFactory.GetFont(LedFontType.Font4x6);
    }

    public void HandleInput(IPlayerConsole player1Console)
    {
        var joystick = player1Console.ReadJoystick();
        player.Move(joystick);

        var buttons = player1Console.ReadButtons();
        player.Shoot(buttons);
    }

    public void Update()
    {
        player.Update();
        enemyWave.Update(player.Bullets);
        if (enemyWave.IsComplete)
        {
            wave++;
            enemyWave = EnemyWaveFactory.CreateWave(wave, this.enemySprite)!;
        }
    }

    public void Draw(IDisplay display)
    {
        player.Draw(display);
        enemyWave.Draw(display);
        font.DrawText(display, 0, 7, Color.Green, "Score: " + player.Score.ToString());
    }

    public GameOverState State
    {
        get
        {
            if (player.IsDestroyed)
            {
                return GameOverState.EndOfGame;
            }
            return GameOverState.None;
        }
    }
}