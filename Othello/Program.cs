using Core.Effects;
using Othello;

Console.WriteLine("Starting Othello...");
var runner = new GameRunner();
runner.Run(() => new OthelloGame());
Console.WriteLine("Exiting Othello...");