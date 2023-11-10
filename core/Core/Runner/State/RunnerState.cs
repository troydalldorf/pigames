namespace Core.Runner.State;

public record RunnerState(string Name, IPlayableGameElement Element, GameState State, Action? Activate = null)
{
    private readonly List<Transition> transitions = new();
    
    public RunnerState AddTransition(RunnerState target, Func<IPlayableGameElement, bool> when)
    {
        transitions.Add(new Transition(target, when));
        return this;
    }

    public RunnerState TryTransition()
    {
        var target = this.transitions
            .Where(x => x.When(this.Element))
            .Select(x => x.Target)
            .FirstOrDefault();
        if (target == null) return this;
        Console.WriteLine($"{Name} => {target.Name}");
        target.Activate?.Invoke();
        return target;
    }
}
