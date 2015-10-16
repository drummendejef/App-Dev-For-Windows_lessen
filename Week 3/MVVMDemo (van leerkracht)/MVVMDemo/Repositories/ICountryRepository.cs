using System.Collections.Generic;
using MVVMDemo.Models;

namespace MVVMDemo.Repositories
{
    public interface ICountryRepository
    {
        List<Country> GetCountries();
    }
}