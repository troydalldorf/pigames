using Core.Display;
using Core.Fonts;
using Core.Inputs;
using Core.PiConsole.Fonts;
using Core.PiConsole.Inputs;
using Core.PiConsole.LedMatrix;
using Microsoft.Extensions.DependencyInjection;

namespace Core.PiConsole;

public static class PiConsoleExtensions
{
    public static IServiceCollection AddPiConsole(this IServiceCollection @this)
    {
        return @this
            .AddTransient<IPlayer1Console, Player1Console>()
            .AddTransient<IPlayer2Console, Player2Console>()
            .AddTransient<IDisplay, LedDisplay>()
            .AddTransient<IFontFactory, FontFactory>();
    }
}