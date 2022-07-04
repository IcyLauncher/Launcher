namespace IcyLauncher.Core.Interfaces;

public interface IConverter
{
    string ToString(object input);

    T ToObject<T>(string input, object? settings = null);

    bool TryToObject<T>(out T result, string input);
}