namespace MastermindGame.Bits;

public class Code
{
    protected const int MaxCodeLength = 4;
    protected static readonly int MaxCodeValue = System.Enum.GetValues<CodeValue>().Length;
    private static readonly Random Random = new Random();
    private readonly CodeValue?[] sequence;

    public Code()
    {
        sequence = new CodeValue?[MaxCodeLength];
    }
    
    public static Code GenerateRandomCode()
    {
        var code = new Code();
        for (var i = 0; i < MaxCodeLength; i++)
        {
            code.sequence[i] = (CodeValue)Random.Next(MaxCodeValue);
        }
        return code;
    }
    
    public bool AnyNull() => sequence.Any(s => s == null);
    
    public CodeValue? this[int index]
    {
        get => sequence[index];
        set => sequence[index] = value;
    }
}