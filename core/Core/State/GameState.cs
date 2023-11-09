namespace Core;

public enum GameState
{
    Playing,
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