using Breakout;
using Core.Effects;

Console.WriteLine("Breakout game starting...");
var runner = new GameRunner();
runner.Run(()=>new BreakoutPlayableGame());
Console.WriteLine("Existing Breakout game...");