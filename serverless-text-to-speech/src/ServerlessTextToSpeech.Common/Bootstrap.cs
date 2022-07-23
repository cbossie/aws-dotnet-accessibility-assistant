using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.Polly;
using Amazon.Textract;
using Amazon.StepFunctions;
using Amazon;

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
        Services.AddAWSService<IAmazonDynamoDB>();
        Services.AddAWSService<IAmazonPolly>();
        Services.AddAWSService<IAmazonTextract>();
        Services.AddAWSService<IAmazonStepFunctions>();

        ServiceProvider = Services.BuildServiceProvider();
    }


}
