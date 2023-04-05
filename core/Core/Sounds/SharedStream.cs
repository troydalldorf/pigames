namespace Core.Sounds;

public class SharedMemoryStream : Stream
{
    private readonly MemoryStream baseStream;
    private long position;

    public SharedMemoryStream(MemoryStream baseStream)
    {
        this.baseStream = baseStream;
        position = 0;
    }

    public override bool CanRead => baseStream.CanRead;
    public override bool CanSeek => baseStream.CanSeek;
    public override bool CanWrite => false;
    public override long Length => baseStream.Length;

    public override long Position
    {
        get => position;
        set => position = value;
    }

    public override void Flush()
    {
        throw new NotSupportedException();
    }

    public override int Read(byte[] buffer, int offset, int count)
    {
        lock (baseStream)
        {
            baseStream.Position = position;
            int bytesRead = baseStream.Read(buffer, offset, count);
            position = baseStream.Position;
            return bytesRead;
        }
    }

    public override long Seek(long offset, SeekOrigin origin)
    {
        long newPosition;

        switch (origin)
        {
            case SeekOrigin.Begin:
                newPosition = offset;
                break;
            case SeekOrigin.Current:
                newPosition = position + offset;
                break;
            case SeekOrigin.End:
                newPosition = baseStream.Length + offset;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(origin));
        }

        if (newPosition < 0 || newPosition > baseStream.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(offset));
        }

        position = newPosition;
        return position;
    }

    public override void SetLength(long value)
    {
        throw new NotSupportedException();
    }

    public override void Write(byte[] buffer, int offset, int count)
    {
        throw new NotSupportedException();
    }
}