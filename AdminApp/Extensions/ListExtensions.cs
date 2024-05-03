namespace AdminApp.Extensions;

public static class ListExtensions
{
    public static int GetItemNumber<T>(this List<T> list, T item)
    {
        return list.IndexOf(item) + 1;
    }
}
