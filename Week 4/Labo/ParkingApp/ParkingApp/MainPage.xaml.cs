using ParkingApp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Calls;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Foundation.Metadata;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace ParkingApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private List<PointOfInterest> points = new List<PointOfInterest>();

        public MainPage()
        {
            this.InitializeComponent();
            LoadParkings();
            map.Loaded += Map_Loaded;
                 map.MapServiceToken = "AqwJfMpLPao81_VFzdwiS3g8P8i9_g1ASdDHEkrPab2FuFSeH6b6kQmCm4UkvuZw";
        }

        private void Map_Loaded(object sender, RoutedEventArgs e)
        {
 
     
            map.Center = new Geopoint(new BasicGeoposition()
            {
                Latitude = 51.054342,
                Longitude = 3.717424
            });
            map.ZoomLevel = 13;
        }

        private async void LoadParkings()
        {
            var results = await ServiceRepository.GetParkings();
            foreach (var parking in results)
            {
                if (parking.parkingStatus == null)
                    continue;

                points.Add(new PointOfInterest()
                {
                    DisplayName = parking.name,
                    FreePlaces = parking.parkingStatus.availableCapacity,
                    Location = new Geopoint(new BasicGeoposition()
                    {
                        Latitude = parking.latitude,
                        Longitude = parking.longitude
                    }),
                    ParkingResult = parking
                });
            }

            mapItems.ItemsSource = points;
        }

        private void btnMapButton_Click(object sender, RoutedEventArgs e)
        {
            pnlInfo.Visibility = Visibility.Visible;
            pnlInfo.DataContext = ((sender as Button).Tag) as ParkingResult;

            //ParkingResult result = (sender as Button).Tag as ParkingResult;
            //Flyout fl = new Flyout();
            //fl.Content = new TextBlock() { Text = "Vrije plaatsen " + result.parkingStatus.availableCapacity.ToString() };
            //fl.ShowAt(sender as FrameworkElement);
        }

        private void TextBlock_Tapped(object sender, TappedRoutedEventArgs e)
        {
            string nr = phoneNr.Text.Split(':')[1].Trim();
            if (ApiInformation.IsApiContractPresent("Windows.ApplicationModel.Calls.CallsPhoneContract", 1, 0))
            {
                PhoneCallManager.ShowPhoneCallUI(nr, "Parking...");
            }

        }
    }
}
