using NAudio.Wave;

namespace Core.Sounds;

public class Sound : IWaveProvider
{
    private readonly byte[] audioData;

    public Sound(string filePath)
    {
        using var audioFileReader = new AudioFileReader(filePath);
        var bytesToRead = (int)audioFileReader.Length;
        var audioBytes = new byte[bytesToRead];
        _ = audioFileReader.Read(audioBytes, 0, bytesToRead);
        WaveFormat = audioFileReader.WaveFormat;
        audioData = audioBytes;
    }
    
    public WaveFormat WaveFormat { get; }
    
    public int? Length => audioData.Length;

    public byte[] GetAudioData()
    {
        return audioData;
    }

    public int Read(byte[] buffer, int offset, int count)
    {
        var bytesToRead = Math.Min(count, audioData.Length);
        Array.Copy(audioData, 0, buffer, offset, bytesToRead);
        return bytesToRead;
    }
}