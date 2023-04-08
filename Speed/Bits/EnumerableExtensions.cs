namespace Speed.Bits;

public static class EnumerableExtensions
{
    private static readonly Random random = new();

    public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
    {
        var elements = source.ToArray();
        for (int i = elements.Length - 1; i > 0; i--)
        {
            int swapIndex = random.Next(i + 1);
            T temp = elements[i];
            elements[i] = elements[swapIndex];
            elements[swapIndex] = temp;
        }

        return elements;
    }
}