using System.Collections.Generic;
using App1.Models;

namespace App1.Repositories
{
    public interface ICountryRepository
    {
        List<Country> GetCountries();
    }
}