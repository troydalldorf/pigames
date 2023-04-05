using SharpAudio;
using SharpAudio.Codec;

namespace Core.Sounds;

public class SoundPlayer : IDisposable
{
    private readonly string path;
    private readonly AudioEngine audioEngine;

    public SoundPlayer(string path)
    {
        this.path = path;
        audioEngine = AudioEngine.CreateDefault();
    }

    public void Play(Sound sound)
    {
        var filepath = Path.Combine(this.path, sound.Filename);

        using var fileStream = File.OpenRead(filepath);
        var memoryStream = new MemoryStream();
        fileStream.CopyTo(memoryStream);
        memoryStream.Position = 0;

        var soundStream = new SoundStream(memoryStream, audioEngine);
        soundStream.Play();
    }

    public void Dispose()
    {
        audioEngine.Dispose();
    }
}