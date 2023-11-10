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
        foreach (var transition in this.transitions)
        {
            if (!transition.When(this.Element)) continue;
            var target = transition.Target;
            Console.WriteLine($"State => {target.Name}");
            target.Activate?.Invoke();
            return target;
        }

        return this;
    }
}
