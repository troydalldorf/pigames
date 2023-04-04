using NAudio.Wave;

namespace Core.Sounds;

public class Player : IDisposable
{
    private readonly WaveOutEvent outputDevice;

    public Player(string path)
    {
        outputDevice = new WaveOutEvent();
    }

    public void Play(Sound sound)
    {
        outputDevice.Init(sound);
        outputDevice.Play();
        outputDevice.Stop();
    }

    public void Dispose()
    {
        outputDevice.Dispose();
    }
}


