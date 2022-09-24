using Newtonsoft.Json;

namespace IcyLauncher.Services;

public class JsonConverter : IConverter
{
    readonly ILogger<JsonConverter> logger;

    public JsonConverter(
        ILogger<JsonConverter> logger)
    {
        this.logger = logger;

        logger.Log("Registered converter");
    }


    public string ToString(
        object input,
        object? formatting = null)
    {
        logger.Log("Serializing object to string");

        return JsonConvert.SerializeObject(input, formatting is Formatting format ? format : Formatting.None);
    }

    public T ToObject<T>(
        string input,
        object? settings = null)
    {
        logger.Log("Deserializing string to object");

        return JsonConvert.DeserializeObject<T>(input, settings is not JsonSerializerSettings ? new() : (JsonSerializerSettings)settings) is T result ?
            result : throw Exceptions.InvalidDeserializedResult;
    }

    public bool TryToObject<T>(
        out T? result,
        string input)
    {
        bool success = true;

        try
        {
            result = ToObject<T>(input,
                new JsonSerializerSettings()
                {
                    Error = (s, e) => (e.ErrorContext.Handled, success) = (true, false),
                    MissingMemberHandling = MissingMemberHandling.Error
                });
        }
        catch
        {
            result = default;
            success = false;
        }

        logger.Log($"Tried to deserialize string to object [{success}]");
        return success;
    }
}