using System;

public static class ArrayExtensions
{
    public static T Find<T>(this T[,] matrix, Predicate<T> match)
    {
        if (matrix == null || match == null)
            throw new ArgumentNullException();

        foreach (var item in matrix)
        {
            if (match(item))
                return item;
        }

        return default;
    }
}
