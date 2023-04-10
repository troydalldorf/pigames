namespace MastermindGame.Bits;

public class Guess : Code
{
    public int CorrectColorAndPosition { get; private set; }
    public int CorrectColorOnly { get; private set; }

    public void NextValue(int cursorPosition)
    {
        var current = (int)(this[cursorPosition] ?? 0);
        this[cursorPosition] = (CodeValue)((current + 1) % MaxCodeValue);
    }
    
    public void PrevValue(int cursorPosition)
    {
        var current = (int)(this[cursorPosition] ?? 0);
        this[cursorPosition] = (CodeValue)((current - 1) % MaxCodeValue);
    }
    
    public void CheckGuess(Code secretCode)
    {
        var correctness = new Correctness[MaxCodeLength];
        var remaining = new List<CodeValue?>();

        for (var i = 0; i < MaxCodeLength; i++)
        {
            if (secretCode[i] == this[i])
                correctness[i] = Correctness.CorrectColorAndPosition;
            else
                remaining.Add(secretCode[i]);
        }

        for (var i = 0; i < MaxCodeLength; i++)
        {
            if (correctness[i] != Correctness.None) continue;
            if (remaining.Contains(this[i]))
            {
                correctness[i] = Correctness.CorrectColorOnly;
                remaining.Remove(this[i]);
            }
        }
        this.CorrectColorAndPosition = correctness.Count(c => c == Correctness.CorrectColorAndPosition);
        this.CorrectColorOnly = correctness.Count(c => c == Correctness.CorrectColorOnly);
    }

    private enum Correctness
    {
        None,
        CorrectColorAndPosition,
        CorrectColorOnly
    }
}