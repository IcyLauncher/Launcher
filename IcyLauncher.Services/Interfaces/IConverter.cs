namespace IcyLauncher.Services.Interfaces;

public interface IConverter
{
    string ToString(object input, object? formatting = null);

    T ToObject<T>(string input, object? settings = null);

    bool TryToObject<T>(out T? result, string input);
}