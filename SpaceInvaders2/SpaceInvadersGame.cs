using Core.Display;
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
    private const int InvaderWidth = 3;
    private const int InvaderHeight = 2;
    private const int PlayerWidth = 5;
    private const int PlayerHeight = 2;
    private const int BulletWidth = 1;
    private const int BulletHeight = 3;

    private LedDisplay display;
    private PlayerConsole playerConsole;
    private Random random = new Random();

    private int playerX;
    private List<Rectangle> invaders;
    private List<Rectangle> bullets;
    private List<PixelBomb> pixelBombs = new();
    private bool moveInvadersRight = true;

    public SpaceInvadersGame(LedDisplay display, PlayerConsole playerConsole)
    {
        this.display = display;
        this.playerConsole = playerConsole;
    }

    public void Run()
    {
        Initialize();

        while (true)
        {
            Update();
            Draw();
            Thread.Sleep(100);
        }
    }

    private void Initialize()
    {
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
        // Update player
        var stick = playerConsole.ReadJoystick();
        if (stick.IsLeft()) playerX--;
        if (stick.IsRight()) playerX++;
        playerX = Math.Clamp(playerX, 0, Width - PlayerWidth);

        // Fire bullet
        if (playerConsole.ReadButtons() > 0 && bullets.Count == 0)
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
                pixelBombs.Add(new PixelBomb(invaders[i].X+2, invaders[i].Y+2, 10, 30, 500));
                bullets.RemoveAt(i);
                invaders.RemoveAt(j);
                break;
            }
        }

        // Update invaders
        var moveX = moveInvadersRight ? 1 : -1;
        var changeDirection = false;

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
                Initialize();
                return;
            }
        }
        foreach (var bomb in pixelBombs.ToArray())
        {
            bomb.Update();
            if (bomb.Complete) pixelBombs.Remove(bomb);
        }
    }

    private void Draw()
    {
        display.Clear();

        // Draw player
        display.DrawRectangle(playerX, Height - PlayerHeight, PlayerWidth, PlayerHeight, Color.White);

        // Draw invaders
        foreach (var invader in invaders)
        {
            display.DrawRectangle(invader.X, invader.Y, invader.Width, invader.Height, Color.Green);
        }

        // Draw bullets
        foreach (var bullet in bullets)
        {
            display.DrawRectangle(bullet.X, bullet.Y, bullet.Width, bullet.Height, Color.Red);
        }

        foreach (var bomb in pixelBombs)
        {
            bomb.Draw(display);
        }
        display.Update();
    }
}