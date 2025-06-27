using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using RapidApiDashboard.Models;
using RapidApiDashboard.Services;

namespace RapidApiDashboard.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ApiService _apiService;

    public HomeController(ILogger<HomeController> logger, ApiService apiService)
    {
        _logger = logger;
        _apiService = apiService;
    }

    public async Task<IActionResult> Index()
    {
        var dashboard = new DashboardViewModel();

        try
        {
            // Tüm API'leri paralel olarak çağır
            var tasks = new List<Task>
            {
                Task.Run(async () => dashboard.CurrencyRates = await _apiService.GetCurrencyRatesAsync()),
                Task.Run(async () => dashboard.FuelPrices = await _apiService.GetFuelPricesAsync()),
                Task.Run(async () => dashboard.Weather = await _apiService.GetWeatherAsync()),
                Task.Run(async () => dashboard.Cryptocurrencies = await _apiService.GetCryptocurrenciesAsync()),
                Task.Run(async () => dashboard.NewsHeadlines = await _apiService.GetNewsAsync()),
                Task.Run(async () => dashboard.DailyRecipe = await _apiService.GetDailyRecipeAsync()),
                Task.Run(async () => dashboard.SportsScores = await _apiService.GetSportsScoresAsync())
            };

            await Task.WhenAll(tasks);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Dashboard API Error: {ex.Message}");
        }

        return View(dashboard);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
