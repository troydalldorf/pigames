namespace Core.Runner;

public interface IGameRunner
{
    void Run(Func<IPlayableGameElement> createGame, GameRunnerOptions options);

    void Run<TGame>(GameRunnerOptions options)
        where TGame : IPlayableGameElement;
}