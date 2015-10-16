using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using MVVMDemo.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVMDemo.ViewModels
{
    public class WeerVM : ViewModelBase
    {
        private ICityRepository repoCities = SimpleIoc.Default.GetInstance<ICityRepository>();

        public List<string> Cities { get; set; }
        public string SelectedCity { get; set; }

        public float SelectedTemperature { get; set; }

        public WeerVM()
        {
            Cities = repoCities.GetCities();
            SelectCityCommand = new RelayCommand(GetTemp);
        }

        private void GetTemp()
        {
            //normaal call naar webservice
            RaisePropertyChanged(() => SelectedTemperature);
        }
        public RelayCommand SelectCityCommand { get; set; }
    }
}
