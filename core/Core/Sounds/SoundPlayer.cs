using SharpAudio;
using SharpAudio.Codec;

namespace Core.Sounds;

public class SoundPlayer : IDisposable
{
    private readonly AudioEngine audioEngine;

    public SoundPlayer()
    {
        audioEngine = AudioEngine.CreateDefault();
    }

    public void Play(Sound sound)
    {
        var soundStream = new SoundStream(sound.GetStream(), audioEngine);
        soundStream.Play();
    }

    public void Dispose()
    {
        audioEngine.Dispose();
    }
}