namespace PrivalgoDigitalSignature.Extensions;

/* The EnumerableExtensions class provides a method to check if an enumerable collection is not null
and contains any elements. */
public static class EnumerableExtensions
{
    /// <summary>
    /// The function checks if an enumerable collection is not null and contains any elements.
    /// </summary>
    /// <param name="enumerable">An IEnumerable<T> object, which is a collection of elements of type
    /// T.</param>
    /// <returns>
    /// The method is returning a boolean value.
    /// </returns>
    public static bool HasAny<T>(this IEnumerable<T> enumerable)
    {
        return enumerable != null && enumerable.Any();
    }
}
