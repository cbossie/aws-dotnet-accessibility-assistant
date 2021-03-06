using Amazon.DynamoDBv2.DataModel;
using Amazon.Lambda.Core;
using Amazon.Lambda.RuntimeSupport;
using Amazon.Lambda.Serialization.SystemTextJson;
using ServerlessTextToSpeech.Common;
using ServerlessTextToSpeech.Common.Model;

// Bootstrap DI Container.
Bootstrap.ConfigureServices();

// The function handler that will be called for each Lambda event
var handler = async (TextToSpeechModel inputModel, ILambdaContext context) =>
{
    var dynamoDBContext = Bootstrap.ServiceProvider.GetRequiredService<IDynamoDBContext>();
    var textToSpeechModel = await dynamoDBContext.LoadAsync<TextToSpeechModel>(inputModel.Id);

    // Update the model
    textToSpeechModel.PollyTaskToken = null;
    textToSpeechModel.PollyJobId = null;
    textToSpeechModel.TaskToken = null;
    textToSpeechModel.TextractJobId = null;

    Uri uri = new Uri(inputModel.PollyOutputUri);
    // The first segment is the URL, and the second segment is the bucket
    textToSpeechModel.SoundKey = string.Join('/', uri.Segments.Skip(2));

    // Save the data
    await dynamoDBContext.SaveAsync(textToSpeechModel);

    return textToSpeechModel;
};

// Build the Lambda runtime client passing in the handler to call for each
// event and the JSON serializer to use for translating Lambda JSON documents
// to .NET types.
await LambdaBootstrapBuilder.Create(handler, new DefaultLambdaJsonSerializer())
        .Build()
        .RunAsync();