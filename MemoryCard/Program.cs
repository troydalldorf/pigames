using Core.Effects;
using MemoryCard;

Console.WriteLine("Memory Card game starting...");
var runner = new GameRunner();
runner.Run(() => new MemoryCardGame());