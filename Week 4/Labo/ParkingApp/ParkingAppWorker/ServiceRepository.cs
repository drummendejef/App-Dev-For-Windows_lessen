using Newtonsoft.Json;
using ParkingApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ParkingAppWorker
{
    internal sealed class ServiceRepository
    {
        public static async Task<ParkingResult[]> GetParkings()
        {
            using (HttpClient client = new HttpClient())
            {
                var result = client.GetAsync("http://datatank4.gent.be//mobiliteit/bezettingparkingsrealtime.json");
                string json = await result.Result.Content.ReadAsStringAsync();
                ParkingResult[] data = JsonConvert.DeserializeObject<ParkingResult[]>(json);
                return data;
            }
        }
    }
}
