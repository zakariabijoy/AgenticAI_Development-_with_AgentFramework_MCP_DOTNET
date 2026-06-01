using Azure.AI.OpenAI;
using Azure.Identity;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;

// 1. Define the variables we extracted from Microsoft Foundry
var endpoint = Environment.GetEnvironmentVariable("AZURE_OPENAI_ENDPOINT") ?? throw new InvalidOperationException("AZURE_OPENAI_ENDPOINT environment variable is not set.");
var model = Environment.GetEnvironmentVariable("AZURE_OPENAI_DEPLOYMENT_NAME") ?? "gpt-5-mini";

// 2. Instantiate the Universal Chat Client
IChatClient chatClient = new AzureOpenAIClient(
    new Uri(endpoint),
    new AzureCliCredential())
    .GetChatClient(model)
    .AsIChatClient();

// 3. create the agent
AIAgent agent = chatClient.AsAIAgent(
    name: "HistoryBuff",
    instructions: "You are a helpful history teacher. Your answers questions and help students make connections between historical events."
);

// 4. Create the Session (The Memory Container)
// This object will accumulate the conversation history.
AgentSession session = await agent.CreateSessionAsync();

Console.WriteLine("History Teacher is online. Type 'exit' to end the conversation.\n");

// 5. The conversation loop
while (true)
{
    Console.Write("User: ");
    string? userInput = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(userInput) || userInput.Equals("exit", StringComparison.OrdinalIgnoreCase))
        break;

    // We pass the 'session' into RunAsync
    // The framwork automatically appends the user's input to this session
    // sends the full history to the cloud and append the agent's response back to the session as well.
    AgentResponse response = await agent.RunAsync(userInput, session);
    Console.WriteLine($"Agent: {response.Text}\n");
}