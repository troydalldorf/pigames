// See https://aka.ms/new-console-template for more information

using BomberMan;
using Core.Effects;

Console.WriteLine("Starting BomberMan...");
var runner = new GameRunner();
runner.Run(() => new BombermanGame());
Console.WriteLine("Exiting BomberMan...");