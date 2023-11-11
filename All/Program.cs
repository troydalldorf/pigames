using All;
using Core;
using Core.PiConsole;
using Core.Runner;
using Microsoft.Extensions.DependencyInjection;

var provider = new ServiceCollection()
    .AddRunner()
    .AddPiConsole()
    .BuildServiceProvider();
var runner = provider.GetRequiredService<IGameRunner>();
runner.Run<Menu>(new GameRunnerOptions(30, false, "Menu"));
