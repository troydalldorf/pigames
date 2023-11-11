using Core.Runner;
using Microsoft.Extensions.DependencyInjection;

namespace Core;

public static class CoreExtensions
{
    public static IServiceCollection AddRunner(this IServiceCollection @this)
    {
        return @this.AddTransient<IGameRunner, GameRunner>();
    }
}