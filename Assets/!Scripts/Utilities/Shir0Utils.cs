using System.Linq;

public static class Shir0Utils
{
    public static T[] TrimExcess<T>(this T[] array)
    {
            T[] trimmed = array.Where(TItem => TItem != null).ToArray();
        return trimmed;
    }
}
