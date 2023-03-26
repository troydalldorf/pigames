using Core.Effects;
using Tetris;

Console.WriteLine("Tetris game starting...");
var runner = new GameRunner();
runner.Run(()=>new DuoPlayableTetrisGame());
Console.WriteLine("Exiting Tetris...");