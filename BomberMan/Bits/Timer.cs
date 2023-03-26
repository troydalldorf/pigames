namespace BomberMan.Bits;

public class Timer
{
    private DateTime _startTime;

    public Timer()
    {
        _startTime = DateTime.Now;
    }

    public void Start()
    {
        _startTime = DateTime.Now;
    }

    public long ElapsedMilliseconds => (long)(DateTime.Now - _startTime).TotalMilliseconds;
}