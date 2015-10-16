using App1.Repositories;
using App1.ViewModel;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App1
{
    public class BaseViewModelLocator//Gaat de Views aan de Viewmodels koppelen, moet ergens aangeroepen worden om aangemaakt te kunnen worden (zie app.xaml
    {
        public BaseViewModelLocator()
        {//Zorgen dat je geen harde verbinding hebt tussen 2 dingen. Objecten injecteren in containers.
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);//In onze Ioc container zal een bepaalde view zitten en gekoppeld worden.
            SimpleIoc.Default.Register<MainPageVM>();//Viewmodels registreren.
            SimpleIoc.Default.Register<WeatherPageVM>();//Viewmodel registreren

            SimpleIoc.Default.Register<ICountryRepository, CountryRepository>();
            //SimpleIoc.Default.Register<IWeerRepository, WeerRepository>();
            SimpleIoc.Default.Register<ICityRepository, CityRepository>();
            
        }

        public MainPageVM MainPageVM
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainPageVM>();//Viewmodels opvragen.
            }
        }

       public WeatherPageVM WeatherPageVM
        {
            get
            {
                return ServiceLocator.Current.GetInstance<WeatherPageVM>();//Viewmodels opvragen
            }
        }
    }
}
