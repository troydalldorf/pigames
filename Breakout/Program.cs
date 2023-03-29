using Breakout;
using Core.Effects;

Console.WriteLine("Breakout game starting...");
var runner = new GameRunner();
runner.Run(()=>new BreakoutGame());
Console.WriteLine("Existing Breakout game...");