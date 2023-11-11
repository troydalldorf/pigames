using Core.Inputs;

namespace Core.PiConsole.Inputs;

public class Player1Console : PlayerConsole, IPlayer1Console
{
    public Player1Console() : base(0x3a, 0x42)
    {
    }
}