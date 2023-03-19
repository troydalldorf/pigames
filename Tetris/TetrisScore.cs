namespace Tetris;

public class TetrisScore
{
    public TetrisScore()
    {
        Score = 0;
    }
    
    public int Score { get; private set; }

    public void Reset()
    {
        Score = 0;
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
    }

    public void ScoreDrop()
    {
        Score++;
    }
}