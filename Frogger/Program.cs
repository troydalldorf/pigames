using Core.Effects;
using Frogger;

Console.WriteLine("Frogger game starting...");
var runner = new GameRunner();
runner.Run(() => new FroggerGame());