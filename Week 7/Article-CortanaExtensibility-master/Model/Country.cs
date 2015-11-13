using System.Collections.Generic;
using Windows.ApplicationModel.Resources;

namespace Model
{
    public sealed class Country
    {
        public string ID { get; set; }
        public string Name { get; set; }

        public string Details { get; set; }

        public string FlagIcon { get; set; }
    }

    public static class Countries
    {
        private static ResourceLoader _resourceLoader = new ResourceLoader();
        private static readonly IList<Country> _list = new List<Country>
    {
        new Country { ID = "1", Name = _resourceLoader.GetString("CountryBelgium"), Details = _resourceLoader.GetString("FlagDetails1"), FlagIcon = "Belgium-flat-icon.png"},
        new Country { ID = "2", Name = _resourceLoader.GetString("CountryFrance"), Details = _resourceLoader.GetString("FlagDetails1"), FlagIcon = "France-flat-icon.png"},
        new Country { ID = "3", Name = _resourceLoader.GetString("CountryUS"), Details = _resourceLoader.GetString("FlagDetails1"), FlagIcon = "United-States-flat-icon.png"},
        new Country { ID = "4", Name = _resourceLoader.GetString("CountryUK"), Details = _resourceLoader.GetString("FlagDetails1"), FlagIcon = "United-Kingdom-flat-icon.png"},
        new Country { ID = "5", Name = _resourceLoader.GetString("CountryBelgium2"), Details = _resourceLoader.GetString("FlagDetails2"), FlagIcon = "Belgium2.png"}
    };
        public static IList<Country> List => _list;

        static Countries()
        {
            _resourceLoader = new ResourceLoader();
        }
    }
}
