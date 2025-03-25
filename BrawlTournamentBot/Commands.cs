using Discord;
using Discord.WebSocket;
using Discord.Interactions;

namespace BrawlTournamentBot;

public class Commands : InteractionModuleBase<SocketInteractionContext>
{
    [SlashCommand("help", "Affiche un message d'aide à propos de ce bot")]
    public async Task Help()
    {
        await Say("L'aide du bot sera bientôt ajouté.", Context.Channel);
    }
    
    [SlashCommand("say", "Fait dire quelque chose au bot")]
    public async Task Say([Summary("Texte", "Le texte qu'il écrira")] string text,
        [Summary("channel", "le channel où il écrira le message")] ISocketMessageChannel? channel = null)
    {
        if (Context != null) //If is called from discord
        {
            if(Equals(channel, null))   channel = Context.Channel;
            
            await channel.SendMessageAsync(text);
        }   
        else                //Is called from the code
        {
            
        }
    }
    
    
    
    
    [SlashCommand("troll", "Ajoute une nom ensuite pour le troll")]
    public async Task Troll([Summary("nom", "le nom à troller")] string name)
    {
        if (name.ToLower().Contains("math"))
        {
            await Say($"Mathe-man en vrai il est pas si mal,\nmais {Context.User.Username} il a le niveau d'une poubelle", Context.Channel);
            return;
        }

        await Say($"{name} est grave nul en vrai");
    }
}