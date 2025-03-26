using Discord;
using Discord.Commands;
using Discord.Interactions;
using Discord.WebSocket;

using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


namespace BrawlTournamentBot;

class Program
{
    private DiscordSocketClient _client = new ();
    private InteractionService _commands;
    private IServiceProvider _services;
    
    private readonly ulong guild_id = 1353741399610974381;
    private readonly string _token = File.ReadLines("token.txt").First();
    static void Main(string[] args) => new Program().RunBotAsync().GetAwaiter().GetResult();

    public async Task RunBotAsync()
    {
        var host = Host.CreateDefaultBuilder()
            .ConfigureServices((context, services) =>
            {
                services.AddSingleton(new DiscordSocketClient(new DiscordSocketConfig()
                {
                    GatewayIntents = GatewayIntents.AllUnprivileged | GatewayIntents.MessageContent
                }));
                services.AddSingleton<InteractionService>();
                services.AddSingleton<CommandHandler>();
            })
            .Build();
        
        using var scope = host.Services.CreateAsyncScope();
        
        _services = scope.ServiceProvider;
        _client = _services.GetRequiredService<DiscordSocketClient>();
        _commands = _services.GetRequiredService<InteractionService>();
        
        _client.Log += LogAsync;
        _client.Ready += ReadyAsync;
        _client.MessageReceived += MessageReceivedAsync;
        _client.ButtonExecuted += Commands.HandleButton;
        
        
        await _client.LoginAsync(TokenType.Bot, _token);
        await _client.StartAsync();

        var commandHandler = _services.GetRequiredService<CommandHandler>();
        await commandHandler.InitializeAsync();

        await Task.Delay(-1); // Garde le bot en ligne
    }

    private Task LogAsync(LogMessage msg)
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

    private async Task ReadyAsync()
    {
        var commandHandler = _services.GetRequiredService<CommandHandler>();
        await commandHandler.RegisterCommandsGuildAsync(guild_id);
    }
}