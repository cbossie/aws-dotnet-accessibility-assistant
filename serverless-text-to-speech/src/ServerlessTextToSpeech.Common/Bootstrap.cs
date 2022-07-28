using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.Polly;
using Amazon.Textract;
using Amazon.StepFunctions;
using Amazon;
using ServerlessTextToSpeech.Common.Services;

namespace ServerlessTextToSpeech.Common;

public static class Bootstrap
{
    private static ServiceCollection Services { get; }
    public static ServiceProvider ServiceProvider { get; private set; }


    static Bootstrap()
    {
        Services = new ServiceCollection();
    }

    public static void ConfigureServices()
    {
        Services.AddDefaultAWSOptions(new()
        {
            Region = RegionEndpoint.GetBySystemName(Environment.GetEnvironmentVariable("AWS_REGION"))
        });

        Services.AddAWSService<IAmazonPolly>();
        Services.AddAWSService<IAmazonTextract>();
        Services.AddAWSService<IAmazonStepFunctions>();

        // DynamoDB and object model
        Services.AddAWSService<IAmazonDynamoDB>();
        Services.AddTransient<IDynamoDBContext>(c => new
            DynamoDBContext(c.GetService<IAmazonDynamoDB>(),
                new DynamoDBContextConfig
                {
                    TableNamePrefix = $"{Environment.GetEnvironmentVariable("STAGE_NAME")}-"
                }));

        // Utilities and internal classes
        Services.AddSingleton<ITextToSpeechUtilities, TextToSpeechUtilities>();

        ServiceProvider = Services.BuildServiceProvider();
    }


}
