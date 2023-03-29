using Core.Effects;

Console.WriteLine("DigDug starting...");
var runner = new GameRunner();
runner.Run(() => new DigDugGame());
Console.WriteLine("Exiting DigDug ...");