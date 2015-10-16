using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using MVVMDemo.Messages;
using MVVMDemo.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace MVVMDemo.ViewModels
{
    public class MainPageVM : ViewModelBase
    {
        public ObservableCollection<string> Cities { get; set; }

        public string SelectedCity { get; set; }

        public WeatherResult CurrentResult { get; set; }

 

        public MainPageVM()
        {

            SelectionChangedCommand = new RelayCommand<SelectionChangedEventArgs>(CitySelectionChanged);
            NextPageCommand = new RelayCommand(NextPage);
            Cities = new ObservableCollection<string>(new CityRepository().GetCities());
           
            RaisePropertyChanged(() => Cities);
            SelectedCity = Cities.First();
            RaisePropertyChanged(() => SelectedCity);
 

            SelectionChangedCommand.Execute(null);
        }

        private async void NextPage()
        {
            GoToPageMessage message = new GoToPageMessage()
            {
                Page = 2
            };
            Messenger.Default.Send<GoToPageMessage>(message);
        }

        private async void CitySelectionChanged(SelectionChangedEventArgs args)
        {
   CurrentResult = await new WeatherRepository().GetWeather(SelectedCity);
 
            RaisePropertyChanged(() => CurrentResult);
 
 
        }

        public RelayCommand<SelectionChangedEventArgs> SelectionChangedCommand { get; set; }

        public RelayCommand NextPageCommand { get; set; }
    }
}
