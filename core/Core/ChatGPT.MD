I have the following interfaces that interact with my Raspberry PI Custom Arcade Console. The Display is 64x64 LED pixels. The inputs are a digital joystick (boolean for up, down, left right) and three buttons, Red, Green, Blue.
The console also supports two players who sit on opposite sites of the display.

public interface IDisplay
{
int Width { get; }
int Height { get; }
void Clear();
void Update();
void SetPixel(int x, int y, Color color);
void DrawCircle(int x, int y, int radius, Color color);
void DrawLine(int x0, int y0, int x1, int y1, Color color);
void DrawRectangle(int x, int y, int width, int height, Color color, Color? fillColor = null);
}

public interface IPlayerConsole
{
JoystickDirection ReadJoystick();
Buttons ReadButtons();
void LightButtons(bool red, bool green, bool blue, bool yellow);
}

public enum JoystickDirection
{
None        = 0x00,
Up          = 0x01,
Down        = 0x02,
Left        = 0x04,
Right       = 0x08,
UpAndLeft   = Up | Left,
UpAndRight  = Up | Right,
DownAndLeft = Down | Left,
DownAndRight= Down | Right,
}

public enum Buttons
{
None = 0x0,
Red = 0x1,
Yellow = 0x2,
Green = 0x4,
Blue = 0x8,
}