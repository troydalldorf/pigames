using CSCore;
using CSCore.Codecs;

namespace Core.Sounds;

public class Sound : IWaveSource
{
    private readonly byte[] audioData;
    private long position;

    public Sound(string filePath)
    {
        using var source = CodecFactory.Instance.GetCodec(filePath);
        var bytesToRead = (int)source.Length;
        var audioBytes = new byte[bytesToRead];
        source.Read(audioBytes, 0, bytesToRead);
        WaveFormat = source.WaveFormat;
        audioData = audioBytes;
    }

    public bool CanSeek => true;
    
    public WaveFormat WaveFormat { get; }

    public long Position
    {
        get => position;
        set => position = Math.Max(0, Math.Min(value, Length));
    }

    public long Length => audioData.Length;

    public byte[] GetAudioData()
    {
        return audioData;
    }

    public int Read(byte[] buffer, int offset, int count)
    {
        var bytesToRead = (int)Math.Min(count, Length - position);
        Array.Copy(audioData, position, buffer, offset, bytesToRead);
        position += bytesToRead;
        return bytesToRead;
    }

    public void Dispose()
    {
    }
}