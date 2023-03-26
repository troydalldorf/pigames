using Core;

namespace All;

public record GameItem(string Name, Func<IPlayableGameElement> CreateGame, int DisplayInterval = 33);