using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using MVVMDemo.Models;
using MVVMDemo.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVMDemo.ViewModels
{
    public class RegistrationVM : ViewModelBase
    {
        private IRegistrationRepository repoReg = SimpleIoc.Default.GetInstance<IRegistrationRepository>();
        public ObservableCollection<Country> Countries { get; set; }
        public Models.Registration NewRegistration { get; set; }
        public ObservableCollection<Models.Registration> Registrations { get; set; }

        private Country _selectedCountry { get; set; }
        public Country SelectedCountry
        {
            get
            {
                return _selectedCountry;
            }
            set
            {
                _selectedCountry = value;
                RaisePropertyChanged(() => SelectedCountry);
            }
        }
        public RegistrationVM()
        {

            NewRegistration = new Models.Registration();
            ICountryRepository repo = SimpleIoc.Default.GetInstance<ICountryRepository>();
            Countries = new ObservableCollection<Country>(repo.GetCountries());
            RaisePropertyChanged(() => Countries);
            SaveCommand = new RelayCommand(Save);
        }

        private void Save()
        {
            NewRegistration.Country = SelectedCountry.Name;
            repoReg.Add(NewRegistration);
            Registrations = new ObservableCollection<Models.Registration>(repoReg.Get());
            RaisePropertyChanged(() => Registrations);
        }

        public RelayCommand SaveCommand { get; set; }


    }
}
