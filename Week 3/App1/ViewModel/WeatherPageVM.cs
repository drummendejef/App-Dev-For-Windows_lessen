using App1.Repositories;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App1.ViewModel
{
    public class WeatherPageVM
    {
        //Properties

        //Lijst van steden opvragen
        private ICityRepository repoCities = SimpleIoc.Default.GetInstance<ICityRepository>();

        //Lijst van steden
        public List<string> Cities { get; set; }

        //Geselecteerde stad
        public string SelectedCity { get; set; }

        //Temperatuur
        public float SelectedTemperature { get; set; }

        //Constructor
        public WeatherPageVM()
        {
            Cities = repoCities.GetCities();
            SelectedCityCommand = new RelayCommand(GetTemp);
        }

        private void GetTemp()
        {
            //RaisePropertyChanged(() => SelectedTemperature);
        }

        public RelayCommand SelectedCityCommand { get; set; }
    }
}
