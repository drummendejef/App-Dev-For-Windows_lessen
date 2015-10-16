using System.Collections.Generic;

namespace MVVMDemo.Repositories
{
    public interface ICityRepository
    {
        List<string> GetCities();
    }
}