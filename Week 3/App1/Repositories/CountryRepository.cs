using App1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App1.Repositories
{
    public class CountryRepository : ICountryRepository
    {
        public List<Country> GetCountries()
        {
            List<Country> returnValue = new List<Country>();
            returnValue.Add(new Country() { ID = 1, Name = "Belgie" });
            returnValue.Add(new Country() { ID = 2, Name = "Holland" });
            returnValue.Add(new Country() { ID = 3, Name = "Duitsland" });
            return returnValue;
        }
    }
}
