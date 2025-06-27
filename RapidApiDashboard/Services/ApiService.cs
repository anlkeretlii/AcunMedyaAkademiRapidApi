using RapidApiDashboard.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace RapidApiDashboard.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public ApiService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<CurrencyData?> GetCurrencyRatesAsync()
        {
            try
            {
                // Exchange Rates API (free version) - USD bazlı
                var response = await _httpClient.GetStringAsync("https://api.exchangerate-api.com/v4/latest/USD");
                var data = JsonConvert.DeserializeObject<JObject>(response);
                
                if (data?["rates"] != null)
                {
                    var rates = data["rates"];
                    var tryRate = rates["TRY"]?.Value<decimal>() ?? 1;
                    
                    return new CurrencyData
                    {
                        USD = Math.Round(tryRate, 2),
                        EUR = Math.Round(tryRate / (rates["EUR"]?.Value<decimal>() ?? 1), 2),
                        GBP = Math.Round(tryRate / (rates["GBP"]?.Value<decimal>() ?? 1), 2),
                        LastUpdated = DateTime.Now
                    };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Currency API Error: {ex.Message}");
            }

            return new CurrencyData { USD = 34.50m, EUR = 37.20m, GBP = 43.80m, LastUpdated = DateTime.Now };
        }

        public async Task<FuelPricesData?> GetFuelPricesAsync()
        {
            try
            {
                // Simulated data - gerçek API'yi implement edebilirsiniz
                return new FuelPricesData
                {
                    Gasoline = 42.50m,
                    Diesel = 39.80m,
                    LPG = 24.30m,
                    LastUpdated = DateTime.Now
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fuel Prices API Error: {ex.Message}");
            }

            return new FuelPricesData { Gasoline = 42.50m, Diesel = 39.80m, LPG = 24.30m, LastUpdated = DateTime.Now };
        }

        public async Task<WeatherData?> GetWeatherAsync()
        {
            try
            {
                // OpenWeatherMap API (free)
                var apiKey = "demo_key"; // Gerçek API key kullanın
                var city = "Istanbul";
                var url = $"https://api.openweathermap.org/data/2.5/weather?q={city}&appid={apiKey}&units=metric&lang=tr";
                
                // Demo data for now
                return new WeatherData
                {
                    City = "İstanbul",
                    Country = "TR",
                    Temperature = 22.5m,
                    Description = "Açık",
                    Humidity = 65,
                    WindSpeed = 3.2m,
                    Icon = "01d"
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Weather API Error: {ex.Message}");
            }

            return new WeatherData 
            { 
                City = "İstanbul", 
                Country = "TR", 
                Temperature = 22.5m, 
                Description = "Açık", 
                Humidity = 65, 
                WindSpeed = 3.2m, 
                Icon = "01d" 
            };
        }

        public async Task<List<CryptocurrencyData>?> GetCryptocurrenciesAsync()
        {
            try
            {
                // CoinGecko API (free)
                var url = "https://api.coingecko.com/api/v3/simple/price?ids=bitcoin,ethereum,binancecoin,cardano,solana&vs_currencies=usd&include_24hr_change=true";
                var response = await _httpClient.GetStringAsync(url);
                var data = JsonConvert.DeserializeObject<JObject>(response);

                var cryptos = new List<CryptocurrencyData>();
                
                if (data != null)
                {
                    var cryptoMap = new Dictionary<string, string>
                    {
                        {"bitcoin", "Bitcoin"},
                        {"ethereum", "Ethereum"},
                        {"binancecoin", "BNB"},
                        {"cardano", "Cardano"},
                        {"solana", "Solana"}
                    };

                    foreach (var crypto in cryptoMap)
                    {
                        if (data[crypto.Key] != null)
                        {
                            var price = data[crypto.Key]?["usd"]?.Value<decimal>() ?? 0;
                            var change = data[crypto.Key]?["usd_24h_change"]?.Value<decimal>() ?? 0;
                            
                            cryptos.Add(new CryptocurrencyData
                            {
                                Name = crypto.Value,
                                Symbol = crypto.Key.ToUpper(),
                                Price = Math.Round(price, 2),
                                Change24h = Math.Round(change, 2)
                            });
                        }
                    }
                }

                return cryptos.Any() ? cryptos : GetDemoCryptoData();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Crypto API Error: {ex.Message}");
                return GetDemoCryptoData();
            }
        }

        private List<CryptocurrencyData> GetDemoCryptoData()
        {
            return new List<CryptocurrencyData>
            {
                new() { Name = "Bitcoin", Symbol = "BTC", Price = 64250.00m, Change24h = 2.5m },
                new() { Name = "Ethereum", Symbol = "ETH", Price = 3420.00m, Change24h = -1.2m },
                new() { Name = "BNB", Symbol = "BNB", Price = 590.00m, Change24h = 0.8m },
                new() { Name = "Cardano", Symbol = "ADA", Price = 0.52m, Change24h = 3.1m },
                new() { Name = "Solana", Symbol = "SOL", Price = 145.00m, Change24h = -0.5m }
            };
        }

        public async Task<List<NewsData>?> GetNewsAsync()
        {
            try
            {
                // RapidAPI News API
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri("https://news-api14.p.rapidapi.com/v2/search/publishers"),
                    Headers =
                    {
                        { "X-RapidAPI-Key", "cc78fe0c86msha5ad1c5ec2de2a5p10e976jsnb392ebddfebc" },
                        { "X-RapidAPI-Host", "news-api14.p.rapidapi.com" },
                    },
                };

                var response = await _httpClient.SendAsync(request);
                
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<JObject>(content);
                    
                    if (data?["data"] != null)
                    {
                        var articles = data["data"].Take(5);
                        var newsList = new List<NewsData>();
                        
                        foreach (var article in articles)
                        {
                            newsList.Add(new NewsData
                            {
                                Title = article["title"]?.ToString() ?? "Başlık bulunamadı",
                                Source = article["publisher"]?["name"]?.ToString() ?? "Kaynak bilinmiyor",
                                Url = article["url"]?.ToString() ?? "#",
                                PublishedAt = DateTime.TryParse(article["published_date"]?.ToString(), out var date) ? date : DateTime.Now
                            });
                        }
                        
                        return newsList;
                    }
                }
                
                // Fallback to demo data if API fails
                return GetDemoNewsData();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"News API Error: {ex.Message}");
                return GetDemoNewsData();
            }
        }

        private List<NewsData> GetDemoNewsData()
        {
            return new List<NewsData>
            {
                new() { Title = "Türkiye Ekonomisinde Son Gelişmeler", Source = "Hürriyet", Url = "#", PublishedAt = DateTime.Now.AddHours(-1) },
                new() { Title = "Teknoloji Sektöründe Yeni Trendler", Source = "TechCrunch", Url = "#", PublishedAt = DateTime.Now.AddHours(-2) },
                new() { Title = "Kripto Para Piyasasında Hareketlilik", Source = "CoinDesk", Url = "#", PublishedAt = DateTime.Now.AddHours(-3) },
                new() { Title = "Otomotiv Sektöründe Elektrikli Araç Atılımı", Source = "Reuters", Url = "#", PublishedAt = DateTime.Now.AddHours(-4) },
                new() { Title = "Yapay Zeka ve Gelecek", Source = "MIT Review", Url = "#", PublishedAt = DateTime.Now.AddHours(-5) }
            };
        }

        public async Task<RecipeData?> GetDailyRecipeAsync()
        {
            try
            {
                // Demo recipe data - Spoonacular API kullanabilirsiniz
                return new RecipeData
                {
                    Title = "Mantı",
                    Image = "https://via.placeholder.com/300x200?text=Manti",
                    ReadyInMinutes = 90,
                    Servings = 4,
                    Instructions = "1. Hamuru hazırlayın\n2. İç harcını karıştırın\n3. Mantıları şekillendirin\n4. Kaynar suda pişirin",
                    Ingredients = new List<string> { "Un", "Yumurta", "Kıyma", "Soğan", "Yoğurt", "Sarımsak" }
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Recipe API Error: {ex.Message}");
            }

            return new RecipeData();
        }

        public async Task<List<SportsScoreData>?> GetSportsScoresAsync()
        {
            try
            {
                // Demo sports data - API-Football veya benzeri servis kullanabilirsiniz
                return new List<SportsScoreData>
                {
                    new() { HomeTeam = "Galatasaray", AwayTeam = "Fenerbahçe", HomeScore = "2", AwayScore = "1", Status = "Bitti", MatchTime = DateTime.Now.AddHours(-2), League = "Süper Lig" },
                    new() { HomeTeam = "Beşiktaş", AwayTeam = "Trabzonspor", HomeScore = "1", AwayScore = "1", Status = "Bitti", MatchTime = DateTime.Now.AddHours(-4), League = "Süper Lig" },
                    new() { HomeTeam = "Manchester City", AwayTeam = "Liverpool", HomeScore = "3", AwayScore = "0", Status = "Bitti", MatchTime = DateTime.Now.AddHours(-6), League = "Premier League" }
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Sports API Error: {ex.Message}");
            }

            return new List<SportsScoreData>();
        }
    }
} 