using Core.Display;
using Core.Display.Sprites;
using Core.Effects;
using Core.Inputs;

namespace SpaceInvaders2;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;

class SpaceInvadersGame
{
    private const int Width = 64;
    private const int Height = 64;
    private const int InvaderWidth = 4;
    private const int InvaderHeight = 3;
    private const int PlayerWidth = 5;
    private const int PlayerHeight = 3;
    private const int BulletWidth = 1;
    private const int BulletHeight = 3;
    private const int MaxBullets = 10;

    private LedDisplay display;
    private PlayerConsole playerConsole;
    private SpriteAnimation alienSprite;
    private SpriteAnimation playerSprite;
    private GameOver gameOver;
    private int alienFrame = 0;
    private int playerX;
    private List<Rectangle> invaders;
    private List<Rectangle> bullets;
    private List<PixelBomb> pixelBombs = new();
    private bool moveInvadersRight = true;

    public SpaceInvadersGame(LedDisplay display, PlayerConsole playerConsole)
    {
        this.display = display;
        this.playerConsole = playerConsole;
        gameOver = new GameOver();
        var image = new SpriteImage("space-invaders.png", new Point(0, 60));
        alienSprite = image.GetSpriteAnimation(0, 0, 4, 3, 2, 1);
        playerSprite = image.GetSpriteAnimation(0, 4, 6, 3, 1, 1);
    }

    public void Run()
    {
        Initialize();
        while (true)
        {
            Update();
            Draw();
        }
    }

    private void Initialize()
    {
        gameOver.State = GameState.Playing;
        playerX = Width / 2 - PlayerWidth / 2;
        invaders = new List<Rectangle>();
        bullets = new List<Rectangle>();

        for (var y = 0; y < 5; y++)
        {
            for (var x = 0; x < 10; x++)
            {
                invaders.Add(new Rectangle(x * 6, y * 4, InvaderWidth, InvaderHeight));
            }
        }
    }

    private void Update()
    {
        if (gameOver.State == GameState.GameOver)
        {
            gameOver.Update(playerConsole);
            if (gameOver.State == GameState.PlayAgain)
                Initialize();
            return;
        }
        // Update player
        var stick = playerConsole.ReadJoystick();
        if (stick.IsLeft()) playerX--;
        if (stick.IsRight()) playerX++;
        playerX = Math.Clamp(playerX, 0, Width - PlayerWidth);

        // Fire bullet
        if (playerConsole.ReadButtons() > 0 && bullets.Count < MaxBullets)
        {
            bullets.Add(new Rectangle(playerX + PlayerWidth / 2 - BulletWidth / 2, Height - PlayerHeight - BulletHeight, BulletWidth, BulletHeight));
        }

        // Update bullets
        for (var i = bullets.Count - 1; i >= 0; i--)
        {
            bullets[i] = new Rectangle(bullets[i].X, bullets[i].Y - 3, bullets[i].Width, bullets[i].Height);

            if (bullets[i].Y < 0)
            {
                bullets.RemoveAt(i);
                continue;
            }

            for (var j = invaders.Count - 1; j >= 0; j--)
            {
                if (!bullets[i].IntersectsWith(invaders[j])) continue;
                pixelBombs.Add(new PixelBomb(invaders[j].X+2, invaders[j].Y+2, 40, Color.GreenYellow));
                bullets.RemoveAt(i);
                invaders.RemoveAt(j);
                break;
            }
        }

        // Update invaders
        var moveX = moveInvadersRight ? 1 : -1;
        var changeDirection = false;
        alienFrame = alienFrame == 0 ? 1 : 0;

        for (var i = 0; i < invaders.Count; i++)
        {
            invaders[i] = new Rectangle(invaders[i].X + moveX, invaders[i].Y, invaders[i].Width, invaders[i].Height);

            if ((moveInvadersRight && invaders[i].Right >= Width) || (!moveInvadersRight && invaders[i].Left <= 0))
            {
                changeDirection = true;
            }
        }

        if (!changeDirection) return;
        {
            moveInvadersRight = !moveInvadersRight;

            for (var i = 0; i < invaders.Count; i++)
            {
                invaders[i] = new Rectangle(invaders[i].X, invaders[i].Y + InvaderHeight, invaders[i].Width, invaders[i].Height);

                if (invaders[i].Bottom < Height - PlayerHeight) continue;
                // Game over
                gameOver.State = GameState.GameOver;
                return;
            }
        }
        foreach (var bomb in pixelBombs.ToArray())
        {
            bomb.Update();
            if (bomb.IsExtinguished()) pixelBombs.Remove(bomb);
        }
    }

    private void Draw()
    {
        display.Clear();
        playerSprite.Draw(display, playerX, Height - PlayerHeight);
        foreach (var invader in invaders)
        {
            alienSprite.Draw(display, invader.X, invader.Y, alienFrame);
        }
        foreach (var bullet in bullets)
        {
            display.DrawRectangle(bullet.X, bullet.Y, bullet.Width, bullet.Height, Color.Red);
        }
        foreach (var bomb in pixelBombs)
        {
            bomb.Draw(display);
        }
        if (gameOver.State == GameState.GameOver)
            gameOver.Draw(display);
        display.Update();
    }
}