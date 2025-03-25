using System;
using System.Threading.Tasks;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;


namespace BrawlTournamentBot;

class Program
{
    private DiscordSocketClient _client;
    private readonly string _token = File.ReadLines("token.txt").First();
    static void Main(string[] args) => new Program().RunBotAsync().GetAwaiter().GetResult();

    public async Task RunBotAsync()
    {
        _client = new DiscordSocketClient(new DiscordSocketConfig()
        {
            GatewayIntents = GatewayIntents.All
        });
        
        _client.Log += Log;
        _client.MessageReceived += MessageReceivedAsync;
        _client.ButtonExecuted += Commands.HandleButton;

        
        await _client.LoginAsync(TokenType.Bot, _token);
        await _client.StartAsync();

        await Task.Delay(-1); // Garde le bot en ligne
    }

    private Task Log(LogMessage msg)
    {
        string log = $"[{DateTime.Now}]: {msg.ToString()}";
        Console.WriteLine(log);
        return Task.CompletedTask;
    }

    private Task Log(string msg)
    {
        string log = $"[{DateTime.Now}]: {msg.ToString()}";
        Console.WriteLine(log);
        return Task.CompletedTask;
    }
    

    private async Task MessageReceivedAsync(SocketMessage message)
    {
        if (message.Author.IsBot || message.Author.IsWebhook || !message.Content.StartsWith("/")) return;
    }

    
}