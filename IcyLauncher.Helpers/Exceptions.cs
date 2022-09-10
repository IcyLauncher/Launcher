namespace IcyLauncher.Helpers;

public class Exceptions
{
    public static readonly Exception InvalidDeserializedResult = new("Deserialized Object returned invalid type or null");

    public static readonly Exception Unsupported = new("Method or object is not supported on this hardware");

    public static readonly Exception UnregisteredType = new("Type is not registered");

    public static readonly Exception IsNull = new("Object is null", new("Object has to be initialized before"));

    public static readonly Exception IsNotNull = new("Object is not null", new("Object has to be not initialized before"));

    public static readonly Exception FileExits = new("File with the same name already exists in destination directory");

    public static readonly Exception FileNotExistsOrLocked = new("File does not exist in destination directory or file is locked");

    public static readonly Exception DirectoryExists = new("Direcotry with the same name already exists");

    public static readonly Exception Timeout = new("Action failed", new("Timeout has been exceeded"));

    public static readonly Exception Cancelled = new("Action failed", new("Cancelled by user"));
}