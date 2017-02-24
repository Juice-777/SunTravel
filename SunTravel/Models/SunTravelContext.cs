using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace SunTravel.Models
{
    public class SunTravelContext : DbContext
    {
        public SunTravelContext():
            base("SunTravel")
        { }
        public DbSet<BookingForm> BookingForms { get; set; }
        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Tour> Tours { get; set; }
    }
}