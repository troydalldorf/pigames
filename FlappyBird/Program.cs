using Core.Effects;
using FlappyBird;

Console.WriteLine("Flappy Bird starting...");
var runner = new GameRunner();
runner.Run(() => new FlappyBirdGame());
Console.WriteLine("Exiting Flappy Bird...");