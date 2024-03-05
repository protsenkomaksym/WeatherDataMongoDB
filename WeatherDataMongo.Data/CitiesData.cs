using MongoDB.Bson.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherDataMongo.Data.Models;
using Newtonsoft.Json;
using SharpCompress.Common;
using MongoDB.Driver;

namespace WeatherDataMongo.Data
{
    public class CitiesData
    {
        /*
         TODO: 
        Move connection to settings
        Optimize the codekhgkh tests 23
         */

        private readonly string _rootPath;
        private string connectionStrig
        {
            get
            {
                return "mongodb://localhost:27017";
            }
        }

        public CitiesData(string rootPath)
        {
            _rootPath = rootPath;
        }

        public void ProcessCitiesJsonData()
        {
            // TODO: Heavy process => async!
            List<Location> locations = LoadJson();

            // Populate MongoDB
            MongoClient mongoClient = new MongoClient(connectionStrig);
            var locationsCollection = mongoClient.GetDatabase("WeatherData").GetCollection<Location>("Locations");

            // Delete all locations before insert
            var filter = Builders<Location>.Filter.Empty;
            locationsCollection.DeleteMany(filter);

            // Insert new
            locationsCollection.InsertMany(locations);
        }

        public List<Location> LoadJson()
        {
            List<Location> city = new List<Location>();
            using (StreamReader r = new StreamReader(Path.Combine(_rootPath, "city.list.json")))
            {
                string json = r.ReadToEnd();
                city = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Location>>(json) ?? new List<Location>();
            }

            return city;
        }

        public IEnumerable<Location> GetWeatherLocations(int? id)
        {
            // TODO: Heavy process => async!

            // Get from DB
            MongoClient mongoClient = new MongoClient(connectionStrig);
            var locationsCollection = mongoClient.GetDatabase("WeatherData").GetCollection<Location>("Locations");

            // Get all locations
            var filter = id == null ? Builders<Location>.Filter.Empty
                : Builders<Location>.Filter.Eq("_id", id.GetValueOrDefault());

            return locationsCollection.Find(filter).ToList();
        }
    }
}
