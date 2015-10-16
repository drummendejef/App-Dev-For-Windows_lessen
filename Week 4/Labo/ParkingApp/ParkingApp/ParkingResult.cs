using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingApp
{

    public class Rootobject
    {
        public ParkingResult[] Results { get; set; }
    }

    public class ParkingResult
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public float latitude { get; set; }
        public float longitude { get; set; }
        public string address { get; set; }
        public string contactInfo { get; set; }
        public bool blurAvailability { get; set; }
        public City city { get; set; }
        public Parkingserver parkingServer { get; set; }
        public int suggestedFreeThreshold { get; set; }
        public int suggestedFullThreshold { get; set; }
        public int capacityRounding { get; set; }
        public Openingtime[] openingTimes { get; set; }
        public Openingtimesinfo openingTimesInfo { get; set; }
        public Parkingstatus parkingStatus { get; set; }
    }

    public class City
    {
        public int id { get; set; }
        public string name { get; set; }
    }

    public class Parkingserver
    {
        public int id { get; set; }
        public string name { get; set; }
    }

    public class Openingtimesinfo
    {
        public int id { get; set; }
        public string code { get; set; }
        public string text { get; set; }
    }

    public class Parkingstatus
    {
        public int availableCapacity { get; set; }
        public int totalCapacity { get; set; }
        public bool open { get; set; }
        public string suggestedCapacity { get; set; }
        public string activeRoute { get; set; }
    }

    public class Openingtime
    {
        public string[] days { get; set; }
        public string from { get; set; }
        public string to { get; set; }
    }

}
