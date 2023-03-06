namespace LedMatrix;

public struct Color
{
    public Color (int r, int g, int b)
    {
        R = (byte)r;
        G = (byte)g;
        B = (byte)b;
    }
    public Color(byte r, byte g, byte b)
    {
        R = r;
        G = g;
        B = b;
    }
    public byte R;
    public byte G;
    public byte B;
}