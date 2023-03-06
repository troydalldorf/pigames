using System.Device.Gpio;
using LedMatrix;

var buttonPin = 27;
var x = 32;
var y = 32;
var color = new Color(155,50,200);

// INPUT SETUP
using GpioController controller = new();
controller.OpenPin(buttonPin, PinMode.InputPullUp);

// MATRIX SETUP
var matrix = new RgbLedMatrix(new RgbLedMatrixOptions
{
    ChainLength = 1,
    Rows = 64,
    Cols = 64,
});
var canvas = matrix.CreateOffscreenCanvas();

while (true)
{
    canvas.SetPixel(x, y, color);
    canvas = matrix.SwapOnVsync(canvas);
    var down = controller.Read(buttonPin);
    if (down == PinValue.Low)
    {
        y = y + 1;
        Thread.Sleep(TimeSpan.FromMilliseconds(50));
    }
} 