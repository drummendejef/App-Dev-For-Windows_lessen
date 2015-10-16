using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.Foundation;

namespace ParkingApp.Models
{
    public class PointOfInterest
    {
        public string DisplayName { get; set; }
        public Geopoint Location { get; set; }
        public Uri ImageSourceUri { get; set; }
        public int FreePlaces { get; set; }
        public string FreePlacesFormatted
        {
            get
            {
                return string.Format("Vrijg plaatsen {0}", FreePlaces);
            }
        }
        public Point NormalizedAnchorPoint { get; set; }
        public ParkingResult ParkingResult { get; set; }
    }
}
