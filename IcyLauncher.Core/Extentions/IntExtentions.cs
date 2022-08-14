namespace IcyLauncher.Core.Extentions;

public static class IntExtentions
{
    public static int RoundDown(this int input, int[] list) =>
        list.Where(x => x <= input) is IEnumerable<int> res && res.Count() > 0 ? res.Last() : 0;
}