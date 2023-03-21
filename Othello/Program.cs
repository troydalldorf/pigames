using Core.Effects;

Console.WriteLine("Starting Othello...");
var runner = new GameRunner();
runner.Run(() => new OthelloGame());
Console.WriteLine("Exiting Othello...");