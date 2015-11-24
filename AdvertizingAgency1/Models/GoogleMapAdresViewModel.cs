using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DAL;

namespace AdvertizingAgency1.Models
{
    public class GoogleMapAdresViewModel
    {
            public int ID { get; set; }
            public string Name { get; set; }
            public int Cost { get; set; }
            public double GeoLong { get; set; } // долгота - для карт google
            public double GeoLat { get; set; } // широта - для карт google
      
    }
}