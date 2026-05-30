using Azure.AI.OpenAI;
using Microsoft.Agents.AI;
using Azure.Identity;
using OpenAI.Chat;

// 1. Define the variables we extracted from Microsoft Foundry
var endpoint = Environment.GetEnvironmentVariable("AZURE_OPENAI_ENDPOINT") ?? throw new InvalidOperationException("AZURE_OPENAI_ENDPOINT environment variable is not set.");
var model = Environment.GetEnvironmentVariable("AZURE_OPENAI_DEPLOYMENT_NAME") ?? "gpt-5-mini";

// 2. Create the Agent Using MAF
AIAgent agent = new AzureOpenAIClient(
    new Uri(endpoint),
    new AzureCliCredential())
    .GetChatClient(model)
    .AsAIAgent(instructions: "You are a friendly AI assistant. Keep your answers brief.");

//Alternative way to create the agent using the api key
//AIAgent agent = new AzureOpenAIClient(
//    new Uri(endpoint),
//    new ApiKeyCredential(apiKey))
//    .GetChatClient(model)
//    .AsAIAgent(instructions: "You are a friendly AI assistant. Keep your answers brief.");

// 3. Invoke the Agent
Console.WriteLine(await agent.RunAsync("What is the largest city in France?"));