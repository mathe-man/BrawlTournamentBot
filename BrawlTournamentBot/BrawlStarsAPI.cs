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

public class BrawlStarsApi
{
    private string apiKey;
    public string myTag = "#LQOUJL8CO";
    
    public BrawlStarsApi(string keyPath = "bs_api_key.txt")
    {
        apiKey = File.ReadLines(keyPath).First();
    }
    
    public async Task TestApi()
    {    
        Console.WriteLine("Testing...");
        BrawlStarsApi api = new BrawlStarsApi();
        string playerTag = myTag;  // Remplace par le tag du joueur que tu veux tester
        Console.WriteLine("Getting results...");
        var result = await api.RequestApi(playerTag);
        
        Console.WriteLine("Results:");
        foreach (var key in result.Keys)
        {
            Console.WriteLine(key + ": " + result[key]);
        }
    }

    public async Task<Dictionary<string, object>> RequestApi(string? playerTag, bool battleLog = false)
    { 
        playerTag ??= myTag;
        string apiUrl = $"https://api.brawlstars.com/v1/players/{playerTag.Replace("#", "%23")}{(battleLog ? "/battlelog" : "")}";

        using (var client = new HttpClient())
        {
            Console.WriteLine("Request" + "1");
            // Add header for api
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + apiKey);
            
            //Send api request
            HttpResponseMessage response = await client.GetAsync(apiUrl);
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Request" + "2");

                string jsonResponse = await response.Content.ReadAsStringAsync();
                Dictionary<string, object> responseObject = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonResponse);
                
                return responseObject;
            }
            string errorContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Erreur API: {response.StatusCode} - {errorContent}");
            return new Dictionary<string, object>();

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