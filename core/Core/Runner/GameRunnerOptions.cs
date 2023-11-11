namespace Core.Runner;

public record GameRunnerOptions(int? FrameIntervalMs, bool CanPause, string Name)
{
    public static GameRunnerOptions Default = new GameRunnerOptions(30, true, "generic");
}