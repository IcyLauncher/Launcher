namespace IcyLauncher.Helpers.Extentions;

public static class IntExtentions
{
    public static int RoundDown(this int input,
        IEnumerable<int> list)
    {
        IEnumerable<int> filtered = list.Where(x => x <= input);
        return filtered.Any() ? filtered.Last() : list.Min();
    }
}