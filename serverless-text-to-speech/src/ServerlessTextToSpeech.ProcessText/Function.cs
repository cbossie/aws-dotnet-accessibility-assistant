using Amazon.Lambda.Core;
using Amazon.Lambda.RuntimeSupport;
using Amazon.Lambda.Serialization.SystemTextJson;
using ServerlessTextToSpeech.Common;
using ServerlessTextToSpeech.Common.Model;
using Amazon.Textract;

// Boostrap DI Container
Bootstrap.ConfigureServices();

// The function handler that will be called for each Lambda event
var handler = async (TextToSpeechModel inputModel, ILambdaContext context) =>
{
    var textractCli = Bootstrap.ServiceProvider.GetRequiredService<IAmazonTextract>();
    var dynamoDBContext = Bootstrap.ServiceProvider.GetRequiredService<IDynamoDBContext>();









};

// Build the Lambda runtime client passing in the handler to call for each
// event and the JSON serializer to use for translating Lambda JSON documents
// to .NET types.
await LambdaBootstrapBuilder.Create(handler, new DefaultLambdaJsonSerializer())
        .Build()
        .RunAsync();