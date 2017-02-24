using SunTravel.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace SunTravel.Models
{
    public class Hotel : IHotel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public int Stars { get; set; }
        public string Description { get; set; }
        public byte[] Photo1 { get; set; }
        public byte[] Photo2 { get; set; }
        public byte[] Photo3 { get; set; }

        //Keys Country
        public int? CountryId { get; set; }
        public Country Country { get; set; }

        //Key tour
        public ICollection<Tour> Tours { get; set; }
        public Hotel()
        {
            Tours = new List<Tour>();
        }
    }
}