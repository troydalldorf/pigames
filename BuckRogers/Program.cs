using BuckRogers;
using Core.Effects;

Console.WriteLine("Buck Rogers starting...");
var runner = new GameRunner();
runner.Run(() => new BuckRogersGame());
Console.WriteLine("Exiting Buck Rogers...");