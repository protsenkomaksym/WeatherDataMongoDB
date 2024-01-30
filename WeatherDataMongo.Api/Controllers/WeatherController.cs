using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System;
using WeatherDataMongo.Data;
using WeatherDataMongo.Data.Models;

namespace WeatherDataMongo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherController : ControllerBase
    {
        private readonly ILogger<WeatherController> _logger;
        private readonly IWebHostEnvironment _environment;

        public WeatherController(ILogger<WeatherController> logger,
            IWebHostEnvironment hostEnvironment)
        {
            _logger = logger;
            _environment = hostEnvironment;
        }

        [HttpPost]
        [Route("GenerateMongoDBData")]
        public IActionResult GenerateMongoDBData()
        {
            // TODO: Move inside Data
            string rootPath = _environment.WebRootPath;
            CitiesData data = new CitiesData(rootPath);

            data.ProcessCitiesJsonData();

            return Ok($"You data has been generated");
        }

        [HttpGet]
        [Route("GetWeatherLocations/{id}")]
        public IActionResult GetWeatherLocations(int? id)
        {
            // TODO: Heavy method! => Optimize
            string rootPath = _environment.WebRootPath;
            CitiesData data = new CitiesData(rootPath);

            return Ok(data.GetWeatherLocations(id));
        }
    }
}
