using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.Lambda.Core;
using Amazon.Lambda.RuntimeSupport;
using Amazon.Lambda.Serialization.SystemTextJson;
using Amazon.Lambda.SNSEvents;
using Amazon.StepFunctions;
using ServerlessTextToSpeech.Common;
using ServerlessTextToSpeech.Common.Model;

// Boostrap DI Container
Bootstrap.ConfigureServices();

// The function handler that will be called for each Lambda event
var handler = async (SNSEvent snsEvent, ILambdaContext context) =>
{
    var dynamoDBContext = Bootstrap.ServiceProvider.GetRequiredService<IDynamoDBContext>();
    var stepFunctionsCli = Bootstrap.ServiceProvider.GetRequiredService<IAmazonStepFunctions>();
    try
    {
        var model = JsonSerializer.Deserialize<NotifyPollyCompleteModel>(snsEvent.Records[0].Sns.Message.ToString());

        // Get the Model for the DynamoDB representation of our job
        var scanData = dynamoDBContext.ScanAsync<TextToSpeechModel>(new List<ScanCondition>
        { 
            new ("pollyjobid", ScanOperator.Equal, model.TaskId)
        });

        var textToSpeechModel = (await scanData.GetNextSetAsync()).FirstOrDefault();
        if(textToSpeechModel is null)
        {
            throw new Exception($"Task not found");
        }

        if ( model.TaskStatus != "completed")
        {
            context.Logger.LogInformation("Sending Failure");
            await stepFunctionsCli.SendTaskFailureAsync(new()
            {
                TaskToken = textToSpeechModel.PollyTaskToken
            });
        }

        context.Logger.LogInformation(JsonSerializer.Serialize(textToSpeechModel));


        // Success. Start up the function again
        context.Logger.LogInformation("Sending Success");

        var jobDataDynamic = new { Payload = textToSpeechModel };
        string jobDataSerialized = JsonSerializer.Serialize(jobDataDynamic);
        context.Logger.LogInformation("JobData:");
        context.Logger.LogInformation(jobDataSerialized);
        await stepFunctionsCli.SendTaskSuccessAsync(new()
        {
            TaskToken = textToSpeechModel.PollyTaskToken,
            Output = jobDataSerialized
        });
    }
    catch (Exception ex)
    {
        context.Logger.LogError(ex.Message);
    }
    context.Logger.LogInformation("Done");
};

// Build the Lambda runtime client passing in the handler to call for each
// event and the JSON serializer to use for translating Lambda JSON documents
// to .NET types.
await LambdaBootstrapBuilder.Create(handler, new DefaultLambdaJsonSerializer())
        .Build()
        .RunAsync();