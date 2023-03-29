namespace Core;

public enum GameState
{
    Starting,
    Playing,
    GameOver,
    Leaderboard,
    Exit,
}

public enum GameOverState
{
    None,
    EndOfGame,
    Player1Wins,
    Player2Wins,
    Draw,
}

public enum GamePauseAction
{
    Resume,
    Paused,
    Exit,
}

public enum GameOverAction
{
    None,
    PlayAgain,
    Exit
}