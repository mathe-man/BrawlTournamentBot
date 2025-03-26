using Discord;
using Discord.WebSocket;
using Discord.Interactions;

namespace BrawlTournamentBot;

public class Commands : InteractionModuleBase<SocketInteractionContext>
{
    [SlashCommand("help", "Affiche un message d'aide à propos de ce bot")]
    public async Task Help()
    {
        
        
        //Create embed
        var helpEmbed = new EmbedBuilder()
            .WithTitle("Brawl Tournament Bot")
            .WithDescription("Ce message est encore un **essaie**")
            .WithFields(new EmbedFieldBuilder().WithIsInline(true).WithValue("Un field"))
            .WithColor(Color.Purple)
            .WithFooter("C'est un bot que j'ai développé")
            .WithTimestamp(DateTime.Now)
            .Build();
        
        //Create delete button
        var component = new ComponentBuilder()
            .WithButton("Supprimer ce message", "delete_embed", ButtonStyle.Primary)
            .Build();
            
            
        await Context.Channel.SendMessageAsync(embed: helpEmbed, components: component);
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


    //Handle buttons events
    public static async Task HandleButton(SocketMessageComponent component)
    {
        if (component.Data.CustomId == "delete_embed")
        {
            await component.Message.DeleteAsync();
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
    
    
    
    
    //Debug commands
    [SlashCommand("display", "Montre un arbre représentant le serveur")]
    public async Task Display()
    {
        SocketGuild guild = Context.Guild;
        string response = "";
        
        response += $"📌 Serveur : {guild.Name} (ID: {guild.Id})\n";

        // Récupérer les rôles
        response += "\n🔹 Rôles disponibles :\n";
        foreach (var role in guild.Roles)
        {
            response += $"  - {role.Name} (ID: {role.Id})";
        }

        // Récupérer les catégories et salons
        response += "\n📂 Catégories et salons :\n\n";

        var categories = guild.Channels
            .Where(c => c is SocketCategoryChannel)
            .Cast<SocketCategoryChannel>();

        foreach (var category in categories)
        {
            response += $"  📁 {category.Name}\n";

            var channels = category.Channels
                .Where(c => c is SocketTextChannel || c is SocketVoiceChannel);

            foreach (var channel in channels)
            {
                string type = channel is SocketTextChannel ? "Text" : "Voice";
                response += $"   | -- #{channel.Name} ({type})\n";
            }

            response += "    --------------------\n\n";
        }
        
        Console.WriteLine(response);
        await Say(response);
    }
    
    
    
}