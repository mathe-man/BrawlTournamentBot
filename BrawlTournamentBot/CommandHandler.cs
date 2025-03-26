using Discord;
using Discord.WebSocket;
using Discord.Interactions;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace BrawlTournamentBot;

public class CommandHandler
{
    private readonly DiscordSocketClient _client;
    private readonly InteractionService _commands;
    private readonly IServiceProvider _services;

    public CommandHandler(DiscordSocketClient client, InteractionService commands, IServiceProvider services)
    {
        _client = client;
        _commands = commands;
        _services = services;
    }

    public async Task InitializeAsync()
    {
        _client.InteractionCreated += HandleInteractionAsync;
        await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
    }

    private async Task HandleInteractionAsync(SocketInteraction interaction)
    {
        var context = new SocketInteractionContext(_client, interaction);
        await _commands.ExecuteCommandAsync(context, _services);
    }

    public async Task RegisterCommandsGlobalAsync()
    {
        // 📌 Enregistre toutes les commandes globalement (prend jusqu'à 1h)
        await _commands.RegisterCommandsGloballyAsync();

        // 📌 OU enregistre pour un serveur spécifique (instantané)
        // Remplace GUILD_ID par l'ID de ton serveur Discord
        // await _commands.RegisterCommandsToGuildAsync(GUILD_ID);
    }

    public async Task RegisterCommandsGuildAsync(ulong guild_id)
    {
        await _commands.RegisterCommandsToGuildAsync(guild_id);
    }
}