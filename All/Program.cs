using All;
using Core.Effects;

Console.WriteLine("Game menu starting...");
var runner = new GameRunner();
runner.Run(()=>new Menu(runner));
Console.WriteLine("Existing menu...");