using Tetris.Bits;

public class SevenBagRandomizer
{
    private List<TetrominoType> bag;
    private static readonly Random Random = new Random();

    public SevenBagRandomizer(Random random)
    {
        InitializeBag();
    }

    private void InitializeBag()
    {
        bag = new List<TetrominoType>
        {
            TetrominoType.I,
            TetrominoType.O,
            TetrominoType.T,
            TetrominoType.S,
            TetrominoType.Z,
            TetrominoType.J,
            TetrominoType.L
        };
    }

    public Tetromino GetRandomTetromino()
    {
        if (bag.Count == 0)
        {
            InitializeBag();
        }

        var randomIndex = Random.Next(bag.Count);
        var randomTetromino = bag[randomIndex];
        bag.RemoveAt(randomIndex);

        return new Tetromino(randomTetromino);
    }
}