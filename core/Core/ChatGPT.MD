I have a Raspberry PI arcade that uses a 64x64 LED matrix display. The arcade is two player, with each player sitting on opposite sides.
The code/classes used to interact with the custom arcade console are described below. The code below also describes the basic code structure.
NOTE that the main game loop is outside of this code, and not something you need to provide.

Please create the following game:

```csharp
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
}

public enum Buttons
{
    None = 0x0,
    Red = 0x1,
    Yellow = 0x2,
    Green = 0x4,
    Blue = 0x8,
}

There is also a class for drawing fonts.

``` csharp
public class LedFont : IDisposable
{
    public LedFont(LedFontType fontType);
    public void DrawText(IDisplay display, int x, int y, Color color, string text, int spacing = 0, bool vertical = false);
}

public enum LedFontType
{
    Font4x6,
    Font5x7,
    Font5x8
}
```

Sample game structure is below. NOTE that the main game loop exists outside the class, that does not need to be included in your output.

``` csharp

public interface IGameElement
{
    void Update();
    void Draw(IDisplay display);
}

public interface IPlayableGameElement : IGameElement
{
    void HandleInput(IPlayerConsole player1Console);
    GameOverState State { get; }
}

public interface IDuoPlayableGameElement : IPlayableGameElement
{
    void Handle2PInput(IPlayerConsole player2Console);
}

public class MyGame : IPlayableGameElement
{
    public MyGame()
    {
        // initialize
    }

    public void HandleInput(IPlayerConsole player1Console)
    {
        // read stick and button information to allow player interaction
    }
    
    public void Handle2PInput(IPlayerConsole player1Console)
    {
        // read stick and button information to allow player interaction
    }

    public void Update()
    {
        // game logic/movement
    }

    public void Draw(IDisplay display)
    {
        // draw to the display, pixels, lines, circles, rectangles, text.
    }

    GameOverState State { get; }
}

public enum GameOverState
{
    None,
    EndOfGame,
    Player1Wins,
    Player2Wins,
    Draw,
}
```