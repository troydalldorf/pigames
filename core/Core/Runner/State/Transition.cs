namespace Core.Runner.State;

public record Transition(RunnerState Target, Func<IPlayableGameElement, bool> When);