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
        
        //id tour 
        //enum status
    }
}