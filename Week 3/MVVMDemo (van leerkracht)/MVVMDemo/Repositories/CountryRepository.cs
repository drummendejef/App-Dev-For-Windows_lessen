using MVVMDemo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVMDemo.Repositories
{
    public class CountryRepository : ICountryRepository
    {
        public List<Country> GetCountries()
        {
            List<Country> lst = new List<Country>();
            lst.Add(new Country()
            {
                Name = "Duitsland"
            });
            lst.Add(new Country()
            {
                Name = "Vlaanderen"
            });
            lst.Add(new Country()
            {
                Name = "Nederland"
            });
            return lst;
        }
    }
}
