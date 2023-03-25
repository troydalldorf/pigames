using Core;

namespace All;

public record GameItem(string Name, Func<IGameElement>? CreateGame);