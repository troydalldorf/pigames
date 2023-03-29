using Core.Effects;
using Minesweeper;

Console.WriteLine("Starting Minesweeper...");
var runner = new GameRunner();
runner.Run(() => new MinesweeperGame());
Console.WriteLine("Exiting Minesweeper...");