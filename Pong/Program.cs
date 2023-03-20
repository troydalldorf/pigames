using Core.Display;
using Core.Effects;
using Core.Inputs;
using Pong;

Console.WriteLine("Pong game starting...");
var runner = new GameRunner();
runner.Run(()=>new PongGame());
Console.WriteLine("Exiting Pong game...");