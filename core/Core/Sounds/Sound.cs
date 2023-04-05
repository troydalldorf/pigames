namespace Core.Sounds;

public class Sound
{
    private readonly MemoryStream memoryStream;
    
    public Sound(string filename)
    {
        using var fileStream = File.OpenRead(filename);
        memoryStream = new MemoryStream();
        fileStream.CopyTo(memoryStream);
        memoryStream.Position = 0;
    }

    public Stream GetStream()
    {
        return new SharedMemoryStream(memoryStream);
    }
}