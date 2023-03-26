using Core.Effects;
using FlappyBird;

Console.WriteLine("Flappy Bird starting...");
var runner = new GameRunner();
runner.Run(() => new FlappyBirdPlayableGame());
Console.WriteLine("Exiting Flappy Bird...");