using Core;

namespace All;

public record GameItem(string Name, Func<IGameElement>? OnePlayer, Func<I2PGameElement>? TwoPlayer);