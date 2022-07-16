var app = new App();
new AwsDotnetAccessibilityAssistantCdkStack(app, "tts-dev", new StackProps
{
    StackName = "AccessibilityAssistantDev",
    Description = "Development Environment for Accessibility Assistant"    
});
app.Synth();
