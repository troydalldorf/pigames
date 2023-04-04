using Core.Display;
using Core.Display.Fonts;
using Core.Runner;

Console.WriteLine("DigDug starting...");
var display = new LedDisplay();
var fontFactory = new FontFactory();
var runner = new GameRunner(display, fontFactory);
runner.Run(() => new DigDugGame(fontFactory));
Console.WriteLine("Exiting DigDug ...");