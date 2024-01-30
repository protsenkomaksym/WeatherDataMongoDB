using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherDataMongo.Data.Models
{
    public class Location
    {
        public long id { get; set; }
        public string? name { get; set; }
        public string? country { get; set; }
        public Coord? coord { get; set; }
    }
}
