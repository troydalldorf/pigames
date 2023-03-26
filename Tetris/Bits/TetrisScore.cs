namespace Tetris.Bits;

public class TetrisScore
{
    public TetrisScore()
    {
        Score = 0;
    }
    
    public int Score { get; private set; }
    
    public int ClearedLines { get; private set; }

    public void Reset()
    {
        Score = 0;
        ClearedLines = 0;
    }
    
    public void ScoreLinesCleared(int lines)
    {
        Score += lines switch
        {
            1 => 100,
            2 => 300,
            3 => 500,
            4 => 800,
            _ => 0,
        };
        ClearedLines += lines;
    }

    public void ScoreDrop()
    {
        Score++;
    }
}