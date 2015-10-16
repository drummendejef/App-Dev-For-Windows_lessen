using Newtonsoft.Json;
using ParkingApp.Models;
using ParkingAppWorker;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.ApplicationModel.Calls;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Foundation.Metadata;
using Windows.Storage;
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
        private const string BackgroundTaskName = "ParkingRefresh";
        private const string BackgroundTaskEntryPoint = "ParkingAppWorker.ServiceWorker";//Entry point is de namespace van de worker en de class. (zie lijst hiernaast)
        private IBackgroundTaskRegistration regTask = null;

        public MainPage()
        {
            this.InitializeComponent();

            map.Loaded += Map_Loaded;
                 map.MapServiceToken = "AqwJfMpLPao81_VFzdwiS3g8P8i9_g1ASdDHEkrPab2FuFSeH6b6kQmCm4UkvuZw";

            SetupBackgroundTask();
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

        private void SetupBackgroundTask()//Om de backgroundtask op te zetten.
        {
            foreach(var task in BackgroundTaskRegistration.AllTasks)//Alle taken in het systeem overlopen
            {
                if(task.Value.Name == BackgroundTaskName)//Weten dat onze backgroundtask is geregistreerd, al bestaat.
                {
                    regTask = task.Value; 
                }
            }

            if(regTask != null)//De taak bestaat al, programma is al een paar keer opgestart.
            {
                regTask.Completed += RegTask_Completed;
            }
            else //Taak is nog niet geregistreerd geweest
            {
                RegisterTask();
            }
        }

        private async void RegisterTask()
        {
            try
            {
                BackgroundAccessStatus status = await BackgroundExecutionManager.RequestAccessAsync();

                if(status == BackgroundAccessStatus.Denied)
                {
                    //Message aan gebruiker, bevoorbeeld
                }
                else
                {
                    //Taak registreren bij systeem
                    BackgroundTaskBuilder builder = new BackgroundTaskBuilder();
                    builder.Name = BackgroundTaskName;
                    builder.TaskEntryPoint = BackgroundTaskEntryPoint;

                    var trigger = new TimeTrigger(15,false);//Oneshot, 1x uitvoeren of elke 15 minuten uitvoeren?
                    builder.SetTrigger(trigger);

                    //Er moet iets zijn, conditie
                    var condition = new SystemCondition(SystemConditionType.InternetAvailable);//Als er internet is
                    builder.AddCondition(condition);

                    regTask = builder.Register();
                    regTask.Completed += RegTask_Completed;
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }

        private async void RegTask_Completed(BackgroundTaskRegistration sender, BackgroundTaskCompletedEventArgs args)
        {
            string json = await readStringFromLocalFile("parking.json");

            ParkingResult[] result = JsonConvert.DeserializeObject<ParkingResult[]>(json);

            foreach(var parking in result)
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
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, RefreshUI);
        }

        //Het uitlezen van de file die de backgroundworker gemaakt had.
        public static async Task<string> readStringFromLocalFile(string filename)
        {
            // reads the contents of file 'filename' in the app's local storage folder and returns it as a string

            // access the local folder
            StorageFolder local = Windows.Storage.ApplicationData.Current.LocalFolder;
            // open the file 'filename' for reading
            Stream stream = await local.OpenStreamForReadAsync(filename);
            string text;

            // copy the file contents into the string 'text'
            using (StreamReader reader = new StreamReader(stream))
            {
                text = reader.ReadToEnd();
            }

            return text;
        }

        public void RefreshUI()
        {
            mapItems.ItemsSource = points;
        }
    }
}
