using Newtonsoft.Json;
using StockPlaform.Dtos.Stock;
using StockPlatform.Interfaces;
using StockPlatform.Mappers;
using StockPlatform.Models;

namespace StockPlaform.Services
{
    public class FMPService : IFMPService
    {
        private readonly HttpClient _httpClient;  // HTTP request bhejne ke liye client
        private readonly IConfiguration _config;   // appsettings.json se API key waghera lene ke liye config

        public FMPService(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;    // constructor injection se httpClient assign kar diya
            _config = config;            // config bhi assign kar diya
        }

        public async Task<Stock> FindStockBySymbolAsync(string symbol)  // Symbol (e.g. "AAPL") ke through stock fetch karega
        {
            try
            {
                // 1. API URL ke sath symbol aur apni API key jorh rahe ho
                var result = await _httpClient.GetAsync($"https://financialmodelingprep.com/api/v3/profile/{symbol}?apikey={_config["FMPApiKey"]}");

                // 2. Check kar rahe ho ke kya API ka response successful aaya ya nahi
                if (result.IsSuccessStatusCode)
                {
                    // 3. Response body ko read kar rahe ho (yeh JSON string milegi)
                    var content = await result.Content.ReadAsStringAsync();

                    // 4. JSON string ko C# object me convert kar rahe ho (list/array of FMPStock)
                    var tasks = JsonConvert.DeserializeObject<FMPStock[]>(content); // Newtonsoft.Json use ho raha yahan

                    // 5. Kyun ke array hai aur har call me 1 item aata hai, isliye first item nikal liya
                    var stock = tasks[0];

                    // 6. Agar stock null nahi hai to usko apne database wale model `Stock` me map karo
                    if (stock != null)
                    {
                        return stock.ToStockFromFMP();  // `ToStockFromFMP()` ek mapper method hai jo FMPStock se Stock me data copy karta hai
                    }

                    // 7. Agar stock null mila to null return kar do
                    return null;
                }

                // 8. Agar response success nahi tha to bhi null return kar do
                return null;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);  // agar koi error aaye (e.g. API down ho) to console me likh do
                return null;
            }
        }
    }
}