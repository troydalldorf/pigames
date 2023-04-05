namespace Core.Sounds;

public class Sound
{
    public string Filename { get; private set; }

    public Sound(string filename)
    {
        Filename = filename;
    }
}