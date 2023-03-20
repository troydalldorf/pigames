using Core.Effects;
using Snake;

Console.WriteLine("Snake game starting...");
var runner = new GameRunner();
runner.Run(new SnakeGame(), 100);
