using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SunTravel.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string User { get; set; }
        public string Manager { get; set; }
        public DateTime DateCreate { get; set; }
        public string DescriptionUser { get; set; }
        public string DescriptionManager { get; set; }
        public Status Status { get; set; }
        public int TourId { get; set; }



        //Key tour
        public ICollection<Tour> Tours { get; set; }
        public Order()
        {
            Tours = new List<Tour>();
        }
    }
    public enum Status : int
    {
        Starting = 0,
        InProcess,
        Completed,
        Canceled
    }
}