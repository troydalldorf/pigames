using CSCore.SoundOut;

namespace Core.Sounds;

public class Player : IDisposable
{
    private readonly ISoundOut outputDevice;

    public Player()
    {
        outputDevice = new WasapiOut();
    }

    public void Play(Sound sound)
    {
        outputDevice.Initialize(sound);
        outputDevice.Play();
        //outputDevice.Stop();
    }

    public void Dispose()
    {
        outputDevice.Dispose();
    }
}


