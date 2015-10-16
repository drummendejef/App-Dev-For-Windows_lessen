using App1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App1.Repositories
{
    class CityRepository : ICityRepository
    {
        public List<string> GetCities()
        {
            List<string> cities = new List<string>();
            cities.Add("London");
            cities.Add("New York");
            cities.Add("Las Vegas");
            cities.Add("Kortrijk");
            cities.Add("San Francisco");
            cities.Add("Bari");
            return cities;
        }
    }
}
