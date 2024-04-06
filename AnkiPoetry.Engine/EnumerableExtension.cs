namespace System.Linq;

public static class EnumerableExtensions
{
    public static IEnumerable<(TItem item, int index)> Indexed<TItem>(this IEnumerable<TItem> items)
        => items.Select((item, index) => (item, index));
}
