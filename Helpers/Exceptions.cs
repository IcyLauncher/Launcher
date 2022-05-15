namespace IcyLauncher.Helpers;

class Exceptions
{
    public static readonly Exception InvalidDeserializedResult = new("Deserialized Object returned invalid type or null");
}