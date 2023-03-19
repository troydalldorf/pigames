using Core.Display.LedMatrix;

namespace Core;

public interface IDirectCanvasAccess
{
    RgbLedCanvas GetCanvas();
}