using Core.Effects;

Console.WriteLine("C4 starting...");
var runner = new GameRunner();
runner.Run(() => new ConnectFourGame());
Console.WriteLine("Exiting C4...");