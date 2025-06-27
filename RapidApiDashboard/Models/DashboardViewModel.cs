using System.ComponentModel.DataAnnotations;

namespace RapidApiDashboard.Models
{
    public class DashboardViewModel
    {
        public CurrencyData? CurrencyRates { get; set; }
        public FuelPricesData? FuelPrices { get; set; }
        public WeatherData? Weather { get; set; }
        public List<CryptocurrencyData>? Cryptocurrencies { get; set; }
        public List<NewsData>? NewsHeadlines { get; set; }
        public RecipeData? DailyRecipe { get; set; }
        public List<SportsScoreData>? SportsScores { get; set; }
    }

    public class CurrencyData
    {
        public decimal USD { get; set; }
        public decimal EUR { get; set; }
        public decimal GBP { get; set; }
        public DateTime LastUpdated { get; set; }
    }

    public class FuelPricesData
    {
        public decimal Gasoline { get; set; }
        public decimal Diesel { get; set; }
        public decimal LPG { get; set; }
        public DateTime LastUpdated { get; set; }
    }

    public class WeatherData
    {
        public string? City { get; set; }
        public string? Country { get; set; }
        public decimal Temperature { get; set; }
        public string? Description { get; set; }
        public decimal Humidity { get; set; }
        public decimal WindSpeed { get; set; }
        public string? Icon { get; set; }
    }

    public class CryptocurrencyData
    {
        public string? Name { get; set; }
        public string? Symbol { get; set; }
        public decimal Price { get; set; }
        public decimal Change24h { get; set; }
        public string? Icon { get; set; }
    }

    public class NewsData
    {
        public string? Title { get; set; }
        public string? Source { get; set; }
        public string? Url { get; set; }
        public DateTime PublishedAt { get; set; }
    }

    public class RecipeData
    {
        public string? Title { get; set; }
        public string? Image { get; set; }
        public int ReadyInMinutes { get; set; }
        public int Servings { get; set; }
        public string? Instructions { get; set; }
        public List<string>? Ingredients { get; set; }
    }

    public class SportsScoreData
    {
        public string? HomeTeam { get; set; }
        public string? AwayTeam { get; set; }
        public string? HomeScore { get; set; }
        public string? AwayScore { get; set; }
        public string? Status { get; set; }
        public DateTime? MatchTime { get; set; }
        public string? League { get; set; }
    }
} 