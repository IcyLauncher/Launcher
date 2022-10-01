using Newtonsoft.Json;

namespace IcyLauncher.Services;

public class JsonConverter : IConverter
{
    readonly ILogger<JsonConverter> logger;

    /// <summary>
    /// Converter which converts json strings and objects
    /// </summary>
    public JsonConverter(
        ILogger<JsonConverter> logger)
    {
        this.logger = logger;

        logger.Log("Registered converter");
    }


    /// <summary>
    /// Converts an object into a json string
    /// </summary>
    /// <param name="input">The object to convert</param>
    /// <param name="formatting">The formatting which will be used to convert the object</param>
    /// <returns>A converted json string of the object</returns>
    /// <exception cref="JsonException">Thrown if JsonConvert.SerializeObject fails</exception>
    public string ToString(
        object input,
        object? formatting = null)
    {
        logger.Log("Serializing object to string");

        return JsonConvert.SerializeObject(input, formatting is Formatting format ? format : Formatting.None);
    }

    /// <summary>
    /// Converts a json stirng into an object
    /// </summary>
    /// <param name="input">The json string to convert</param>
    /// <returns>A converted object of the json string</returns>
    /// <exception cref="Exceptions.InvalidDeserializedResult">Thrown if converted object is not type T</exception>
    /// <exception cref="JsonException">Thrown if JsonConvert.DeserializeObject fails</exception>
    public T ToObject<T>(
        string input)
    {
        logger.Log("Deserializing string to object");

        return JsonConvert.DeserializeObject<T>(input) is T result ?
            result : throw Exceptions.InvalidDeserializedResult;
    }

    /// <summary>
    /// Tries to convert a json string into an object 
    /// </summary>
    /// <param name="result">The converted converted object of the json string</param>
    /// <param name="input">The json string to convert</param>
    /// <returns>A boolean wether the conversation was successful</returns>
    public bool TryToObject<T>(
        out T? result,
        string input)
    {
        bool success = true;

        result = JsonConvert.DeserializeObject<T>(input,
            new JsonSerializerSettings()
            {
                Error = (s, e) =>
                {
                    success = false;
                    e.ErrorContext.Handled = true;
                },
                MissingMemberHandling = MissingMemberHandling.Error
            }) is T res ? res : default;

        logger.Log($"Tried to deserialize string to object [{success}]");
        return success;
    }
}