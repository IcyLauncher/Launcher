namespace IcyLauncher.Core.Extentions;

public static class ExceptionExtentions
{
    public static string Format(this Exception? input) =>
        input is null ? "" :
        input.InnerException is null ? $": {input.Message}" :
        $": {input.Message}\n\t{input.InnerException.Message.Replace("\n", "\n\t")}";
}