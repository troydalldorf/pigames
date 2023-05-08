using System.Drawing;
using Core;
using Core.Display.Sprites;
using Core.Effects;
using Core.Fonts;
using PacificWings.Bits;

namespace PacificWings;

public class PacificWingsGame : IPlayableGameElement
{
    private readonly Player player;
    private Ocean ocean;
    private EnemyWave enemyWave;
    private readonly SpriteAnimation enemySprite;
    private readonly Explosions explosions = new();
    private readonly IFont font;
    private int wave = 1;

    public PacificWingsGame(IFontFactory fontFactory)
    {
        this.ocean = new Ocean();
        var image = SpriteImage.FromResource("pwings.png");
        var playerSprite = image.GetSpriteAnimation(1, 1, 9, 8, 2, 1);
        this.enemySprite = image.GetSpriteAnimation(1, 14, 9, 8, 2, 1);
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
        this.ocean.Update();
        this.player.Update();
        this.explosions.Update();
        this.enemyWave.Update(player.Bullets, player);
        if (enemyWave.IsComplete)
        {
            wave++;
            enemyWave = EnemyWaveFactory.CreateWave(wave, this.enemySprite)!;
        }
    }

    public void Draw(IDisplay display)
    {
        this.ocean.Draw(display);
        this.player.Draw(display);
        this.enemyWave.Draw(display);
        this.explosions.Draw(display);
        this.font.DrawText(display, 0, 7, Color.Green, player.Score.ToString());
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