using System.Drawing;
using Core.Display;
using Core.Display.Sprites;
using Core.Inputs;

var image = new SpriteImage("graphics.png", new Point(0, 0));
var alient1Sprite = image.GetSpriteAnimation(0, 0, 8, 8, 3, 1);
var alient2Sprite = image.GetSpriteAnimation(0, 117, 8, 8, 3, 1);
var alient3Sprite = image.GetSpriteAnimation(0, 36, 8, 8, 3, 1);
var army = new AlienArmy(alient1Sprite, alient2Sprite, alient3Sprite);
var p1 = image.GetSpriteAnimation(27, 27, 16, 8, 1, 1);
var phaserSprite = image.GetSpriteAnimation(27, 0, 8, 8, 4, 1);
var display = new LedDisplay();
var player = new Player(0x3a, 0x42);
var phasers = new Phasers(phaserSprite);
var p1x = 20;
var frame = 0;
while (true)
{
    // animation
    alient1Sprite.Frame = alient2Sprite.Frame = alient3Sprite.Frame = (frame/10) % 2;
    phaserSprite.Frame = (frame / 4) % 4;
    
    // motion
    var stick = player.ReadJoystick();
    if (stick.IsLeft()) p1x--;
    if (stick.IsRight()) p1x++;
    var buttons = player.ReadButtons();
    if (buttons > 0) phasers.Add(p1x+7, 57, frame);
    p1x = Math.Max(0, Math.Min(48, p1x));
    phasers.Move(-1);
    
    // draw
    display.Clear();
    army.Draw(display);
    phasers.Draw(display);
    display.DrawSprite(p1x, 57, p1);
    display.Update();
    frame++;
}

public class AlienArmy
{
    private bool[,] aliens;
    private SpriteAnimation row1;
    private SpriteAnimation row2;
    private SpriteAnimation row3;
    
    public AlienArmy(SpriteAnimation row1, SpriteAnimation row2, SpriteAnimation row3)
    {
        this.row1 = row1;
        this.row2 = row2;
        this.row3 = row3;
        aliens = new bool[3, 6];
        for (var y = 0; y < 3; y++)
        {
            for (var x = 0; x < 6; x++)
            {
                aliens[y, x] = true;
            }
        }
    }

    public void Draw(LedDisplay display)
    {
        for (var y = 0; y < 3; y++)
        {
            for (var x = 0; x < 6; x++)
            {
                display.DrawSprite(x*10, y*10, y switch { 0 => row1, 1 => row2, _ => row3 });
            }
        }
    }
}

public class Phasers
{
    private readonly SpriteAnimation sprite;
    private List<Phaser> phasers = new();
    private int lastFrame = 0;
    
    public Phasers(SpriteAnimation sprite)
    {
        this.sprite = sprite;
    }

    public void Move(int deltaY)
    {
        foreach (var phaser in phasers.ToArray())
        {
            phaser.Move(deltaY);
            if (phaser.IsComplete()) phasers.Remove(phaser);
        }
    }

    public void Add(int x, int y, int frame)
    {
        if (frame - lastFrame > 10)
        {
            phasers.Add(new Phaser(x, y));
            lastFrame = frame;
        }
    }

    public void Draw(LedDisplay display)
    {
        foreach (var phaser in phasers)
        {
            display.DrawSprite(phaser.X, phaser.Y, sprite);
        }
    }
}

public class Phaser
{
    public int X { get; private set; }
    public int Y { get; private set; }

    public Phaser(int x, int y)
    {
        X = x;
        Y = y;
    }

    public void Move(int deltaY)
    {
        Y += deltaY;
    }

    public bool IsComplete()
    {
        return Y < -6;
    }
}