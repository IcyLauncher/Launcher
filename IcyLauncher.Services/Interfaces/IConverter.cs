namespace IcyLauncher.Services.Interfaces;

public interface IConverter
{
    /// <summary>
    /// Converts an object into a string
    /// </summary>
    /// <param name="input">The object to convert</param>
    /// <param name="formatting">The formatting which will be used to convert the object</param>
    /// <returns>A converted json string of the object</returns>
    /// <exception cref="JsonException"> Thrown if Converter.SerializeObject fails (JsonConverter => JsonException...)</exception>
    string ToString(object input, object? formatting = null);

    /// <summary>
    /// Converts a stirng into an object
    /// </summary>
    /// <param name="input">The string to convert</param>
    /// <returns>A converted object of the string</returns>
    /// <exception cref="Exceptions.InvalidDeserializedResult">Thrown if converted object is not type T</exception>
    /// <exception cref="Exception">Thrown if Converter.DeserializeObject fails (JsonConverter => JsonException...)</exception>
    T ToObject<T>(string input);

    /// <summary>
    /// Tries to convert a string into an object 
    /// </summary>
    /// <param name="result">The converted converted object of the string</param>
    /// <param name="input">The string to convert</param>
    /// <returns>A boolean if the conversation was successful</returns>
    bool TryToObject<T>(out T? result, string input);
}