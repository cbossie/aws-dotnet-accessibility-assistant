using Amazon.Lambda.Core;
using Amazon.Lambda.RuntimeSupport;
using Amazon.Lambda.Serialization.SystemTextJson;
using ServerlessTextToSpeech.Common;
using System.Text.Json;

// Boostrap DI
Bootstrap.ConfigureServices();


// The function handler that will be called for each Lambda event
var handler = async (ServerlessTextToSpeech.Common.TextToSpeechModel inputModel, ILambdaContext context) =>
{
    context.Logger.LogInformation(JsonSerializer.Serialize(inputModel));
    //Tests
    return inputModel;
};

// Build the Lambda runtime client passing in the handler to call for each
// event and the JSON serializer to use for translating Lambda JSON documents
// to .NET types.
await LambdaBootstrapBuilder.Create(handler, new DefaultLambdaJsonSerializer())
        .Build()
        .RunAsync();