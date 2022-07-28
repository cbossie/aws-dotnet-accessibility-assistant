using Amazon.Lambda.Core;
using Amazon.Lambda.RuntimeSupport;
using Amazon.Lambda.Serialization.SystemTextJson;
using ServerlessTextToSpeech.Common;
using ServerlessTextToSpeech.Common.Model;
using Amazon.Textract;
using Amazon.Textract.Model;

// Boostrap DI Container
Bootstrap.ConfigureServices();

// The function handler that will be called for each Lambda event
var handler = async (TextToSpeechModel inputModel, ILambdaContext context) =>
{
    var textractCli = Bootstrap.ServiceProvider.GetRequiredService<IAmazonTextract>();

    //Continuation Token
    List<Block> resultBlocks = new List<Block>();
    string? token = null;

    do
    {
        var data = await textractCli.GetDocumentTextDetectionAsync(new()
        {
            JobId = inputModel.TextractJobId,
            MaxResults = 1000,
            NextToken = token

        });

        token = data.NextToken;
        resultBlocks.AddRange(data.Blocks);


    } while (token is not null);


    context.Logger.LogInformation("HERE ARE MY RESULTS");
    context.Logger.LogInformation(JsonSerializer.Serialize(resultBlocks));
};

// Build the Lambda runtime client passing in the handler to call for each
// event and the JSON serializer to use for translating Lambda JSON documents
// to .NET types.
await LambdaBootstrapBuilder.Create(handler, new DefaultLambdaJsonSerializer())
        .Build()
        .RunAsync();