using Core.Display;
using Core.Display.Sprites;
using Core.Inputs;

var image = new SpriteImage("graphics.png");
var alien1 = image.GetSpriteAnimation(0, 0, 8, 8, 3, 1);
var alien2 = image.GetSpriteAnimation(0, 9, 8, 8, 3, 1);
var alien3 = image.GetSpriteAnimation(0, 36, 8, 8, 3, 1);
var army = new AlienArmy(alien1, alien2, alien3);
var p1 = image.GetSpriteAnimation(27, 27, 16, 8, 1, 1);
var phaser = image.GetSpriteAnimation(27, 0, 8, 8, 4, 1);
var display = new LedDisplay();
var player = new Player(0x3a, 0x42);
var p1x = 20;
var frame = 0;
while (true)
{
    alien1.Frame = alien2.Frame = alien3.Frame = (frame/10) % 2;
    var stick = player.ReadJoystick();
    p1x += stick switch
    {
        JoystickDirection.Left => -1,
        JoystickDirection.Right => +1,
        _ => 0,
    };
    p1x = Math.Max(0, Math.Min(56, p1x));
    display.Clear();
    army.Draw(display);
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
