using Microsoft.AspNetCore.Mvc;
using Tracker.WebAPIClient;

namespace Rad302SampleExam2024.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", 
            "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            ActivityAPIClient.Track(
                StudentID: "S00219971",
                StudentName: "Ulas Karamustafaoglu",
                activityName: "Rad302 Mock Exam 2025",
                Task: "Fixing WeatherForecastController for Swagger"
            );

            _logger = logger;
        }

        // ✅ Fix for Swagger/OpenAPI: Add explicit method attribute
        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
                {
                    Date = DateTime.Now.AddDays(index),
                    TemperatureC = Random.Shared.Next(-20, 55),
                    Summary = Summaries[Random.Shared.Next(Summaries.Length)]
                })
                .ToArray();
        }
    }
}