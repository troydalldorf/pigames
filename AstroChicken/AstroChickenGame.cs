using System.Drawing;
using Core;
using Core.Display.Sprites;
using Core.Fonts;

namespace AstroChicken;

public class AstroChicken : IDuoPlayableGameElement
{
    private readonly IFont font;
    private readonly Chicken p1Chicken;
    private readonly Chicken p2Chicken;
    private const int ChickenSize = 4;
    private readonly Sprite introSprite;
    
    public AstroChicken(IFontFactory fontFactory)
    {
        var introImage = SpriteImage.FromResource("intro.png");
        introSprite = introImage.GetSprite(0, 0, 64, 44);
        font = fontFactory.GetFont(LedFontType.Font5x7);
        p1Chicken = new Chicken(5, 32, ChickenSize, Color.Red);
        p2Chicken = new Chicken(64 - 5 - ChickenSize, 32, ChickenSize, Color.Blue);
        State = GameOverState.None;
    }

    public void HandleInput(IPlayerConsole player1Console)
    {
        var direction = player1Console.ReadJoystick();
        if (direction.HasFlag(JoystickDirection.Up))
        {
            p1Chicken.MoveUp();
        }
        if (direction.HasFlag(JoystickDirection.Down))
        {
            p1Chicken.MoveDown();
        }
    }
    
    public void Handle2PInput(IPlayerConsole player2Console)
    {
        JoystickDirection direction = player2Console.ReadJoystick();
        if (direction.HasFlag(JoystickDirection.Up))
        {
            p2Chicken.MoveUp();
        }
        if (direction.HasFlag(JoystickDirection.Down))
        {
            p2Chicken.MoveDown();
        }
    }

    public void Update()
    {
        // Check for collisions and update game state accordingly
        if (p1Chicken.CollidesWith(p2Chicken))
        {
            State = GameOverState.Draw;
        }
    }

    public void Draw(IDisplay display)
    {
        introSprite.Draw(display, 0, 10);
        // Draw the chickens
        p1Chicken.Draw(display);
        p2Chicken.Draw(display);

        // Draw the scores using the font
        font.DrawText(display, 5, 5, Color.White, $"P1");
        font.DrawText(display, 55, 5, Color.White, $"P2");
    }

    public GameOverState State { get; private set; }
}

public class Chicken
{
    public float X { get; private set; }
    public float Y { get; private set; }
    public int Size { get; private set; }
    public Color Color { get; private set; }

    public Chicken(float x, float y, int size, Color color)
    {
        X = x;
        Y = y;
        Size = size;
        Color = color;
    }

    public void MoveUp()
    {
        Y -= Size;
    }

    public void MoveDown()
    {
        Y += Size;
    }

    public void Draw(IDisplay display)
    {
        display.DrawRectangle((int)X, (int)Y, Size, Size, Color, Color);
    }

    public bool CollidesWith(Chicken other)
    {
        return X < other.X + other.Size && X + Size > other.X && Y < other.Y + other.Size && Y + Size > other.Y;
    }
}
