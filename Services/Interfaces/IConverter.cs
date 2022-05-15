namespace IcyLauncher.Services.Interfaces;

interface IConverter
{
    string ToString(object input);

    void SaveToPath(object input, string path);

    T ToObject<T>(string input, object? settings = null);

    bool TryToObject<T>(out T result, string input);
}