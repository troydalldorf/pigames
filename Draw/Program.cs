using Core.Inputs;

Console.WriteLine("Starting...");
var player1 = new Player(0x3a);

while (true)
{
    Console.WriteLine(player1.ReadJoystick().ToString());
}