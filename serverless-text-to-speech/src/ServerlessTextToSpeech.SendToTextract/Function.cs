using Amazon.Lambda.Core;
using Amazon.Lambda.RuntimeSupport;
using Amazon.Lambda.Serialization.SystemTextJson;
using Amazon.Textract;
using Amazon.Textract.Model;
using Amazon.DynamoDBv2.DataModel;

using ServerlessTextToSpeech.Common;
using ServerlessTextToSpeech.Common.Model;
using System.Text.Json;

// Boostrap DI Container
Bootstrap.ConfigureServices();


// The function handler that will be called for each Lambda event
var handler = async (TextToSpeechModel inputModel, ILambdaContext context) =>
{
    // Get Required Services
    var textractCli = Bootstrap.ServiceProvider.GetRequiredService<IAmazonTextract>();
    var dynamoDBContext = Bootstrap.ServiceProvider.GetRequiredService<IDynamoDBContext>();

    //Submit to Textract
    var startDocProcessResult = await textractCli.StartDocumentTextDetectionAsync(new()
    {
        DocumentLocation = new()
        {
            S3Object = new()
            {
                Bucket = inputModel.BucketName,
                Name = inputModel.ObjectKey
            }
        },
        OutputConfig = new()
        {
            S3Bucket = Environment.GetEnvironmentVariable("OUTPUT_BUCKET"),
            S3Prefix = Environment.GetEnvironmentVariable("OUTPUT_PREFIX")
        },
        NotificationChannel = new()
        {
            SNSTopicArn = Environment.GetEnvironmentVariable("TEXTRACT_TOPIC"),
            RoleArn = Environment.GetEnvironmentVariable("TEXTRACT_ROLE")
        },
        JobTag = inputModel.Id
    });


    inputModel.TextractJobId = startDocProcessResult.JobId;

    //Save the data to the DB

    await dynamoDBContext.SaveAsync(inputModel);

    return inputModel;
};

// Build the Lambda runtime client passing in the handler to call for each
// event and the JSON serializer to use for translating Lambda JSON documents
// to .NET types
await LambdaBootstrapBuilder.Create(handler, new DefaultLambdaJsonSerializer())
        .Build()
        .RunAsync();