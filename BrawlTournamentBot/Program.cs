﻿using System;
using System.Threading.Tasks;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;

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

        await _client.LoginAsync(TokenType.Bot, _token);
        await _client.StartAsync();

        await Task.Delay(-1); // Garde le bot en ligne
    }

    private Task Log(LogMessage msg)
    {
        Console.WriteLine(msg.ToString());
        return Task.CompletedTask;
    }

    private async Task MessageReceivedAsync(SocketMessage message)
    {
        if (message.Author.IsBot || message.Author.IsWebhook || !message.Content.StartsWith("/")) return;
        string[] command = message.Content.Remove(0, 1).Split(" ");

        if (command[0] == "troll")
        {
            if (command.Length == 1)
            {await SendMessage("Tu parle de qui ??", message.Channel); return;}
            
            if (command[1].ToLower().Contains("math"))
            {
                await SendMessage("Mathe-man en vrai c'est le goat", message.Channel);
                await Troll(message.Author.Username, message.Channel);
                return;
            }

            await Troll(command[1], message.Channel);
        }
        
    }

    private async Task SendMessage(string message, ISocketMessageChannel channel)
    {
        await channel.SendMessageAsync(message);
    }
    
    //Commands
    private async Task Troll(string name, ISocketMessageChannel channel)
    {
        await SendMessage($"{name} est plutôt nul sur le jeu",  channel);
    }
}