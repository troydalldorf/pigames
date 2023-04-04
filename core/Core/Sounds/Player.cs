using NAudio.Wave;

namespace Core.Sounds;

public class Player : IDisposable
{
    private readonly WaveOutEvent outputDevice;

    public Player()
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


