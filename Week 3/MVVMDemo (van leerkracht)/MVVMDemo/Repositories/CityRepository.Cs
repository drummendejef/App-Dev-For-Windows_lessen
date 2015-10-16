 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVMDemo.Repositories
{
    public class CityRepository : ICityRepository
    {
        public List<string> GetCities()
        {
            List<string> cities = new List<string>();
            cities.Add("London");
            cities.Add("New York");
            cities.Add("Las Vegas");
            cities.Add("Kortrijk");
            cities.Add("San Fransisco");
            cities.Add("Bari");
            return cities;
        }
    }
}
