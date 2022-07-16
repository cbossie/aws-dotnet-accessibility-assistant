using Amazon.CDK.AWS.S3;
using Amazon.CDK.AWS.Lambda;

namespace AwsDotnetAccessibilityAssistantCdk;

public class AwsDotnetAccessibilityAssistantCdkStack : Stack
{
    internal AwsDotnetAccessibilityAssistantCdkStack(Construct scope, string id, IStackProps props = null) : base(scope, id, props)
    {
        // Bucket
        Bucket inputBucket = new(this, "inputBucket", new BucketProps 
        {
            BucketName = $"{id}-input-bucket"
        });

        Bucket outputBucket = new(this, "outputBucket", new BucketProps
        {
            BucketName = $"{id}-output-bucket"
        });


        // Lambda Functions
        




    }
}
