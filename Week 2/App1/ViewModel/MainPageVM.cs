using App1.Messages;
using App1.Models;
using App1.Repositories;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App1.ViewModel
{
    public class MainPageVM : ViewModelBase
    {
        private ICountryRepository repoCountries = SimpleIoc.Default.GetInstance<ICountryRepository>();

        public List<Country> Countries { get; set; }

        public Registration NewRegistration { get; set; }

        public ObservableCollection<Registration> Registrations { get; set; }

        public MainPageVM()
        {
            //Delegate = variabele waar een methode inzit.

            Countries = repoCountries.GetCountries();
            SaveCommand = new RelayCommand(Save);
            Registrations = new ObservableCollection<Registration>();
            NewRegistration = new Registration();
            NextPageCommand = new RelayCommand(GotoNewPage);
        }

        private void GotoNewPage()
        {
            GoToPageMessage message = new GoToPageMessage() { PageNumber = 1 };
            Messenger.Default.Send<GoToPageMessage>(message);
        }

        private void Save()
        {
            Registrations.Add(NewRegistration);
        }


        public RelayCommand SaveCommand
        {
            get; set;
        }

        public RelayCommand NextPageCommand
        {
            get; set;
        }
    }
}
