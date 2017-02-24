using SunTravel.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;


namespace SunTravel.Models
{
    public class Country : ICountry
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public byte[] Photo1 { get; set; }
        public string Description { get; set; }

        //Key hotel
        public ICollection<Hotel> Hotels { get; set; }

        //Key tour
        public ICollection<Tour> Tours { get; set; }

        public Country()
        {
            Hotels = new List<Hotel>();
            Tours = new List<Tour>();
        }

    }
}