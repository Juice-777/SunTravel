using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SunTravel.Models
{
    public interface IHotel
    {
        int Id { get; set; }
        string Name { get; set; }
        int Stars { get; set; }
        string Description { get; set; }
        byte[] Photo1 { get; set; }
        byte[] Photo2 { get; set; }
        byte[] Photo3 { get; set; }
    }
}