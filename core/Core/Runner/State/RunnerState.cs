namespace Core.Runner.State;

public record RunnerState(string Name, IPlayableGameElement Element, GameState State, Action? Activate = null, params Transition[] Transitions)
{
    public RunnerState AddTransition(RunnerState target, Func<IPlayableGameElement, bool> when)
    {
        return this with
        {
            Transitions = this.Transitions.Concat(new[] { new Transition(target, when) }).ToArray()
        };
    }

    public RunnerState TryTransition()
    {
        Console.WriteLine($"TryTransition from {this.Name}: {this.Transitions.Length}");
        foreach (var transition in this.Transitions)
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
