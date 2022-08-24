namespace IcyLauncher.Helpers.Extentions;

public static class IntExtentions
{
    public static int RoundDown(this int input, int[] list) =>
        list.Where(x => x <= input) is IEnumerable<int> res && res.Any() ? res.Last() : 0;
}