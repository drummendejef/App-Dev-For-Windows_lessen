using Newtonsoft.Json;
using ParkingApp;
using ParkingApp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.Storage;

namespace ParkingAppWorker
{
    public sealed class ServiceWorker : IBackgroundTask //Sealed is dat het vanuit verschillende talen aangesproken kan worden.
    {
        public async void Run(IBackgroundTaskInstance taskInstance)//Code die hier in komt gaat uitgevoerd worden in de achtergrond.
        {
            if(BackgroundWorkCost.CurrentBackgroundWorkCost == BackgroundWorkCostValue.High)//Als het te druk is, niet uitvoeren
            {
                return;
            }

            BackgroundTaskDeferral deferral = taskInstance.GetDeferral();//Systeem laten weten dat je iets async gaat doen

            //De parkings ophalen en opslaan.
            ParkingResult[] result = await ServiceRepository.GetParkings();
            var json = JsonConvert.SerializeObject(result);
            await saveStringToLocalFile("parking.json", json);
            deferral.Complete();
        }

        //Dingen wegschrijven naar de harde schijf
        async Task saveStringToLocalFile(string filename, string content)
        {
            // saves the string 'content' to a file 'filename' in the app's local storage folder
            byte[] fileBytes = System.Text.Encoding.UTF8.GetBytes(content.ToCharArray());

            // create a file with the given filename in the local folder; replace any existing file with the same name
            StorageFile file = await Windows.Storage.ApplicationData.Current.LocalFolder.CreateFileAsync(filename, CreationCollisionOption.ReplaceExisting);

            // write the char array created from the content string into the file
            using (var stream = await file.OpenStreamForWriteAsync())
            {
                stream.Write(fileBytes, 0, fileBytes.Length);
            }
        }
    }
}
