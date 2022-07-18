using System.Text.Json;
using System.Text.Json.Serialization;

namespace ServerlessTextToSpeech.Common;

public class TextToSpeechModel
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("bucket")]
    public string? BucketName { get; set; }

    [JsonPropertyName("key")]
    public string? ObjectKey { get; set; }

    [JsonPropertyName("time")]
    public DateTime? RequestTime { get; set; }

    [JsonPropertyName("region")]
    public string? Region { get; set; }  
}