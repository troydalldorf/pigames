using Core.Effects;

Console.WriteLine("C4 starting...");
var runner = new GameRunner();
runner.Run(() => new AsteroidsGame());
Console.WriteLine("Exiting C4...");