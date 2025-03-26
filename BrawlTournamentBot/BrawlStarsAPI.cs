using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;


namespace BrawlTournamentBot;

public class Player
{
    [JsonProperty("tag")]
    public string Tag { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("brawler")]
    public Brawler Brawler { get; set; }
}

public class Brawler
{
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("power")]
    public int Power { get; set; }

    [JsonProperty("trophies")]
    public int Trophies { get; set; }
}



public class BattleEvent
{
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("mode")]
    public string Mode { get; set; }

    [JsonProperty("map")]
    public string Map { get; set; }
}

public class Battle
{
    [JsonProperty("mode")]
    public string Mode { get; set; }

    [JsonProperty("type")]
    public string Type { get; set; }

    [JsonProperty("result")]
    public string Result { get; set; }

    [JsonProperty("trophyChange")]
    public int TrophyChange { get; set; }

    [JsonProperty("duration")]
    public int Duration { get; set; }

    [JsonProperty("starPlayer")]
    public Player StarPlayer { get; set; }

    [JsonProperty("teams")]
    public List<List<Player>> Teams { get; set; }
}

public class ApiResponse
{
    [JsonProperty("items")]
    public List<ApiItem> Items { get; set; }

}
public class ApiItem
{
    [JsonProperty("battleTime")]
    public string BattleTime { get; set; }

    [JsonProperty("event")]
    public BattleEvent Event { get; set; }

    [JsonProperty("battle")]
    public Battle Battle { get; set; }
}

public class BrawlStarsAPI
{
    private string apiKey;
    public string myTag = "%23LQOUJL8CO";
    
    public BrawlStarsAPI(string keyPath = "bs_api_key.txt")
    {
        apiKey = File.ReadLines(keyPath).First();
    }
    
    public async Task TestApi()
    {        
        BrawlStarsAPI api = new BrawlStarsAPI();
        string playerTag = myTag;  // Remplace par le tag du joueur que tu veux tester
        var result = await api.GetLastBattleAsync(playerTag);

        if (result.Item1 != null && result.Item2 != null)
        {
            BattleEvent battleEvent = result.Item1;
            Battle battle = result.Item2;

            Console.WriteLine($"Last Battle Event: {battleEvent.Mode} on {battleEvent.Map}");
            Console.WriteLine($"Battle Type: {battle.Type}, Result: {battle.Result}");
            Console.WriteLine($"Star Player: {battle.StarPlayer?.Name}");
        }
        else
        {
            Console.WriteLine("Aucun combat n'a été trouvé ou un problème est survenu.");
        }
    }

    public async Task<(BattleEvent, Battle)> GetLastBattleAsync(string playerTag)
    {
        string apiUrl = $"https://api.brawlstars.com/v1/players/{playerTag}/battlelog";
        using (HttpClient client = new HttpClient())
        {
            // Ajouter l'en-tête pour l'API Key
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + apiKey);

            // Faire la requête GET vers l'API
            HttpResponseMessage response = await client.GetAsync(apiUrl);
            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                // Désérialiser la réponse JSON
                var battleLogs = JsonConvert.DeserializeObject<List<BattleEvent>>(jsonResponse);

                // Si il y a des logs de bataille, renvoyer le dernier
                if (battleLogs != null && battleLogs.Count > 0)
                {
                    BattleEvent lastBattleEvent = battleLogs[0];
                    Battle lastBattle = await GetBattleDetailsAsync(lastBattleEvent.Id);  // Récupérer les détails du combat

                    return (lastBattleEvent, lastBattle);
                }
                else
                {
                    Console.WriteLine("Aucun combat trouvé pour ce joueur.");
                    return (null, null);
                }
            }
            else
            {
                Console.WriteLine("Erreur de l'API : " + response.StatusCode);
                return (null, null);
            }
        }
    }
    
    private async Task<Battle> GetBattleDetailsAsync(long battleEventId)
    {
        // Faire un appel API pour obtenir les détails du combat par son ID
        using (HttpClient client = new HttpClient())
        {
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + apiKey);
            HttpResponseMessage response = await client.GetAsync($"https://api.brawlstars.com/v1/battles/{battleEventId}");
            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Battle>(jsonResponse);
            }
            else
            {
                Console.WriteLine("Erreur de l'API pour les détails du combat.");
                return null;
            }
        }
    }
}