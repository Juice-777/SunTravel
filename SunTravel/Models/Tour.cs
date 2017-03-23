using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using SunTravel.Models;
using System.IO;


namespace SunTravel.Models
{
    public class Tour : ITour
    {
        //ITour
        public int Id { get; set; } //count place in room
        public string Name { get; set; }
        [Display(Name = "Places in room")]
        public int Place { get; set; } //count place in room
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd'/'MM'/'yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Start date")]
        public DateTime DateStart { get; set; }
        public int Duration { get; set; } //продолжительность путёвки
        [Display(Name = "Price tour")]
        public decimal Price { get; set; }
        public int FreePlace { get; set; } //количество свободных путёвок
        //[Display(Name = "Photo")]
        public byte[] Photo1 { get; set; }
        public Active Active { get; set; }

        //IService
        public bool Insurance { get; set; }
        public Food Food { get; set; }

        //Key country
        public int? CountryId { get; set; }
        public Country Country { get; set; }

        //Key hotel
        public int? HotelId { get; set; }
        public Hotel Hotel { get; set; }

        //Key order
        public int? OrderId { get; set; }
        public Order Order { get; set; }
    }
    public enum Food : int
    {
        None = 0,
        Light,
        Morning,
        Full
    }
    public enum Active : int
    {
        Show = 0,
        Frost,
        Disabled
    }
}