using System.Text.Json;
using System.Text.Json.Serialization;
using Amazon.DynamoDBv2.DataModel;


namespace ServerlessTextToSpeech.Common.Model;

[DynamoDBTable("TextToSpeechData")]
public class TextToSpeechModel
{

    [DynamoDBHashKey]
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

    [JsonPropertyName("jobid")]
    public string? TextractJobId { get; set; }

    [JsonPropertyName("tasktoken")]
    public string? TaskToken {get;set;}
}