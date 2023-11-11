using Core;
using Core.Display;
using Core.Display.Sprites;
using Core.Effects;
using Core.Inputs;
using Core.State;
using SpaceInvaders2.Bits;

namespace SpaceInvaders2;

using System;
using System.Collections.Generic;
using System.Drawing;

public class SpaceInvadersPlayableGame : IPlayableGameElement
{
    private const int Width = 64;
    private const int Height = 64;

    private const int PlayerWidth = 5;
    private const int PlayerHeight = 3;
    private const int MaxBullets = 10;
    
    private Levels levels;
    private Level currentLevel;
    private readonly SpriteAnimation playerSprite;
    private int alienFrame = 0;
    private int playerX;
    private List<Rectangle> bullets;
    private readonly Explosions explosions = new();
    private bool moveInvadersRight = true;

    public SpaceInvadersPlayableGame()
    {
        var image = SpriteImage.FromResource("si.png", new Point(0, 60));
        levels = new Levels(image);
        playerSprite = image.GetSpriteAnimation(0, 4, 6, 3, 1, 1);
        playerX = Width / 2 - PlayerWidth / 2;
        bullets = new List<Rectangle>();
        NextWave();
    }

    public GameOverState State { get; private set; }

    private void NextWave()
    {
        currentLevel = levels.CreateLevel1();
    }

    public void HandleInput(IPlayerConsole playerConsole)
    {
        var stick = playerConsole.ReadJoystick();
        var buttons = playerConsole.ReadButtons();
        if (stick.IsLeft()) playerX--;
        if (stick.IsRight()) playerX++;
        playerX = Math.Clamp(playerX, 0, Width - PlayerWidth);

        // Fire bullet
        if (buttons.IsGreenPushed() && bullets.Count < MaxBullets)
        {
            bullets.Add(new Rectangle(playerX + PlayerWidth / 2 - Levels.BulletWidth / 2, Height - PlayerHeight - Levels.BulletHeight, Levels.BulletWidth, Levels.BulletHeight));
        }
    }

    public void Update()
    {
        explosions.Update();
        
        // Update projectiles
        for (var i = bullets.Count - 1; i >= 0; i--)
        {
            bullets[i] = new Rectangle(bullets[i].X, bullets[i].Y - 3, bullets[i].Width, bullets[i].Height);
            if (bullets[i].Y < 0)
            {
                bullets.RemoveAt(i);
                continue;
            }
            
            for (var j = currentLevel.Enemies.Count - 1; j >= 0; j--)
            {
                if (!bullets[i].IntersectsWith(currentLevel.Enemies[j].Rectangle)) continue;
                explosions.Explode(currentLevel.Enemies[j].Rectangle.X + currentLevel.Enemies[j].Sprite.Width/2, currentLevel.Enemies[j].Rectangle.Y + currentLevel.Enemies[j].Sprite.Height/2);
                bullets.RemoveAt(i);
                currentLevel.Enemies[j].Health -= 1;
                if (currentLevel.Enemies[j].Health <= 0)
                {
                    currentLevel.Enemies.RemoveAt(j);
                }
                break;
            }
        }
        
        if (!currentLevel.Enemies.Any())
        {
            NextWave();
            return;
        }

        // Update invaders
        var moveX = moveInvadersRight ? currentLevel.Speed : -currentLevel.Speed;
        var changeDirection = false;
        alienFrame = alienFrame == 0 ? 1 : 0;

        foreach (var t in currentLevel.Enemies)
        {
            t.Rectangle = new Rectangle(t.Rectangle.X + moveX, t.Rectangle.Y, t.Rectangle.Width, t.Rectangle.Height);
            if ((moveInvadersRight && t.Rectangle.Right >= Width) || (!moveInvadersRight && t.Rectangle.Left <= 0))
            {
                changeDirection = true;
            }
        }

        if (!changeDirection) return;
        {
            moveInvadersRight = !moveInvadersRight;

            for (var i = 0; i < currentLevel.Enemies.Count; i++)
            {
                currentLevel.Enemies[i].Rectangle = new Rectangle(currentLevel.Enemies[i].Rectangle.X, currentLevel.Enemies[i].Rectangle.Y + Levels.InvaderHeight, currentLevel.Enemies[i].Rectangle.Width, currentLevel.Enemies[i].Rectangle.Height);

                if (currentLevel.Enemies[i].Rectangle.Bottom < Height - PlayerHeight) continue;
                // Game over
                State = GameOverState.EndOfGame;
                return;
            }
        }
    }

    // Modify the Draw method to draw the level
    public void Draw(IDisplay display)
    {
        playerSprite.Draw(display, playerX, Height - PlayerHeight);
        foreach (var enemy in currentLevel.Enemies)
        {
            enemy.Sprite.Draw(display, enemy.Rectangle.X, enemy.Rectangle.Y, alienFrame);
        }
        foreach (var bullet in bullets)
        {
            display.DrawRectangle(bullet.X, bullet.Y, bullet.Width, bullet.Height, Color.Red);
        }
        explosions.Draw(display);
    }
}