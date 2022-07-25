using Amazon.Lambda.Core;
using Amazon.Lambda.RuntimeSupport;
using Amazon.Lambda.Serialization.SystemTextJson;
using Amazon.Textract;
using Amazon.Textract.Model;
using Microsoft.Extensions.DependencyInjection;
using ServerlessTextToSpeech.Common;
using System.Text.Json;

// Boostrap DI
Bootstrap.ConfigureServices();


// The function handler that will be called for each Lambda event
var handler = async (ServerlessTextToSpeech.Common.TextToSpeechModel inputModel, ILambdaContext context) =>
{
    context.Logger.LogInformation(JsonSerializer.Serialize(inputModel));
    var textractCli = Bootstrap.ServiceProvider.GetRequiredService<IAmazonTextract>();

    var startDocProcessResult = await textractCli.StartDocumentAnalysisAsync(new()
    { 
        DocumentLocation = new ()
        {
            S3Object = new ()
            {
                Bucket = inputModel.BucketName,
                Name = inputModel.ObjectKey
            }
        },
        OutputConfig = new ()
        {
            S3Bucket = Environment.GetEnvironmentVariable("OUTPUT_BUCKET"),
            S3Prefix = Environment.GetEnvironmentVariable("OUTPUT_PREFIX")
        },
        NotificationChannel = new ()
        {
            SNSTopicArn = Environment.GetEnvironmentVariable("TEXTRACT_TOPIC"),
            RoleArn = Environment.GetEnvironmentVariable("TEXTRACT_ROLE")
        },
        FeatureTypes = new (),
        JobTag = inputModel.Id
    });





    return inputModel;
};

// Build the Lambda runtime client passing in the handler to call for each
// event and the JSON serializer to use for translating Lambda JSON documents
// to .NET types.
await LambdaBootstrapBuilder.Create(handler, new DefaultLambdaJsonSerializer())
        .Build()
        .RunAsync();