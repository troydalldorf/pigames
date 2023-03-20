using Core.Effects;
using Tetris;

Console.WriteLine("Tetris game starting...");
var runner = new GameRunner();
runner.Run(new DuoTetrisGame());
Console.WriteLine("Exiting Tetris...");