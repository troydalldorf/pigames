using Core.Inputs;

namespace Core.PiConsole.Inputs;

public class Player2Console : PlayerConsole, IPlayer2Console
{
    public Player2Console() : base(0x44, 0x43)
    {
    }
}