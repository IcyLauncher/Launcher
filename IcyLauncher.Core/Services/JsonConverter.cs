using Newtonsoft.Json;

namespace IcyLauncher.Core.Services;

public class JsonConverter : IConverter
{
    readonly ILogger logger;

    public JsonConverter(ILogger<JsonConverter> logger)
    {
        this.logger = logger;

        this.logger.Log("Registered Converter (Json)");
    }


    public string ToString(object input)
    {
        logger.Log("Serializing object to string");
        return JsonConvert.SerializeObject(input);
    }

    public void SaveToPath(object input, string path)
    {
        logger.Log($"Serializing object to path ({path})");
        throw new NotImplementedException();
    }

    public T ToObject<T>(string input, object? settings = null)
    {
        logger.Log("Deserializing string to object");
        return (JsonConvert.DeserializeObject<T>(input, settings is not JsonSerializerSettings ? new() : (JsonSerializerSettings)settings) is T result) ?
            result : throw Exceptions.InvalidDeserializedResult;
    }

    public bool TryToObject<T>(out T result, string input)
    {
        bool success = true;

        result = ToObject<T>(input,
            new JsonSerializerSettings()
            {
                Error = (s, e) => (e.ErrorContext.Handled, success) = (true, false),
                MissingMemberHandling = MissingMemberHandling.Error
            });

        logger.Log($"Tried to deserialize string to object (Status: {success})");
        return success;
    }
}