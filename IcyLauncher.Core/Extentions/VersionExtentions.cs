namespace IcyLauncher.Core.Extentions;

public static class VersionExtentions
{
    public static Version TrimZeros(this Version input) =>
        input.Revision == 0 || input.Revision == -1 ?
        input.Build == 0 || input.Build == -1 ?
        new(input.Major, input.Minor) :
        new(input.Major, input.Minor, input.Build) :
        input;
}