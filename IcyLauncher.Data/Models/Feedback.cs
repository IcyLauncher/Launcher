using Newtonsoft.Json;

namespace IcyLauncher.Data.Models;

public class Feedback
{
    [JsonIgnore]
    public FeedbackResult Result { get; set; } = FeedbackResult.Cancel;
    public double? Stars { get; set; }
    public string? Content { get; set; }
    public string? Account { get; set; }
}